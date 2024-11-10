using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Application.Core.Interfaces;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Application.Core.Shared.Enums;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.Services
{
    public class OrderCalculatorService : IOrderCalculatorService
    {
        private readonly IValidator<OrderDTO> _orderValidator;
        private readonly IClientRepository _clientRepository;
        private readonly ITaxRateRepository _taxRateRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IGenericRepository<Order> _orderGenericRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<OrderResult> _orderResultGenericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderCalculatorService(
            IValidator<OrderDTO> orderValidator,
            IClientRepository clientRepository,
            ITaxRateRepository taxRateRepository,
            IProductRepository productRepository,
            ICouponRepository couponRepository,
            IPromotionRepository promotionRepository,
            IGenericRepository<Order> orderGenericRepository,
            IMapper mapper,
            IGenericRepository<OrderResult> orderResultGenericRepository,
            IUnitOfWork unitOfWork
            )
        {
            _orderValidator = orderValidator;
            _clientRepository = clientRepository;
            _taxRateRepository = taxRateRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _promotionRepository = promotionRepository;
            _orderGenericRepository = orderGenericRepository;
            _mapper = mapper;
            _orderResultGenericRepository = orderResultGenericRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Create the Order based on Product and calculate TotalCost,TaxAmount and PreTaxTotal.
        /// </summary>
        /// <param name="orderDTO">Order details</param>
        /// <returns>Returns order result after save Order. </returns>
        /// <exception cref="ValidationException">
        /// Thrown when <paramref name="orderDTO"/> has 0 products, Quantity as 0.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when there is an error retrieving the state, stateTax and products.
        /// </exception>
        public OrderResultDTO CalculateOrderTax(OrderDTO orderDTO)
        {
            ValidateOrderDTO(orderDTO);

            State? state = _clientRepository.GetStateNameForClient(orderDTO.ClientName);
            if (state == null)
            {

                throw new Exception("Invalid Client found");
            }

            TaxRate? taxRate = _taxRateRepository.GetTaxRatesByState(state.StateName);
            if (taxRate == null)
            {
                throw new Exception("Invalid StateTax Rate");
            }

            List<string> productNames = orderDTO.Products.Select(x => x.Name).ToList();
            Dictionary<string, Product> productsDictionary = _productRepository.GetProducts(productNames);
            if (productsDictionary == null || orderDTO.Products.Count != productsDictionary.Count)
            {
                throw new Exception("Invalid products");
            }
            Promotion? promotion = _promotionRepository.GetMaximumPromotionalDiscountByDate(orderDTO.OrderDate);
            Dictionary<int, decimal> couponsDictionary = _couponRepository.GetCouponsByProductIdsAndDate(productNames, orderDTO.OrderDate);

            OrderResultDTO result = new OrderResultDTO();
            decimal totalOrderBeforeDiscountTax = 0;
            decimal totalCouponDiscount = 0;
            decimal totalPromotionalDiscount = 0;
            decimal TotalTax = 0;
            foreach (ProductDTO orderProduct in orderDTO.Products)
            {
                decimal productTotalPrice = 0;
                if (productsDictionary != null && productsDictionary.TryGetValue(orderProduct.Name, out Product? productDetails))
                {
                    productTotalPrice = productDetails.Price * orderProduct.Quantity;
                    totalOrderBeforeDiscountTax += productTotalPrice;

                    decimal productDiscount = 0;
                    decimal productPromotionalDiscount = 0;
                    decimal productTax = 0;
                    int productTaxRate = CalculateTaxRate(taxRate, productDetails);
                    productDiscount = CalculateProductCouponDiscount(couponsDictionary, productTotalPrice, productDetails);
                    totalCouponDiscount += productDiscount;

                    productPromotionalDiscount = CalculateProductPromotionalDiscount(promotion, productTotalPrice, productDiscount);
                    totalPromotionalDiscount += productPromotionalDiscount;
                    productTax = CalculateProductTax(taxRate, productTotalPrice, productDiscount, productPromotionalDiscount, productTaxRate);
                    TotalTax += productTax;
                }

            }
            result.TotalCost = totalOrderBeforeDiscountTax - totalCouponDiscount - totalPromotionalDiscount + TotalTax;
            result.PreTaxTotal = totalOrderBeforeDiscountTax - totalCouponDiscount - totalPromotionalDiscount;
            result.TaxAmount = TotalTax;

            _orderGenericRepository.Add(new Order());
            OrderResult orderResult = _mapper.Map<OrderResult>(result);
            _orderResultGenericRepository.Add(orderResult);
            _unitOfWork.SaveChanges();
            return result;
        }

        private void ValidateOrderDTO(OrderDTO orderDTO)
        {
            ValidationResult validationResult = _orderValidator.Validate(orderDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }


        private decimal CalculateProductCouponDiscount(Dictionary<int, decimal> couponsDictionary, decimal productTotalPrice, Product productDetails)
        {
            if (couponsDictionary != null && couponsDictionary.TryGetValue(productDetails.ID, out decimal discount))
            {
                return productTotalPrice * (discount / 100);

            }
            return 0;
        }
        private decimal CalculateProductPromotionalDiscount(Promotion? promotion, decimal productTotalPrice, decimal productDiscount)
        {
            if (promotion != null)
            {
                return (productTotalPrice - productDiscount) * ((decimal)promotion.DiscountPercentage / 100);

            }
            return 0;
        }
        private static decimal CalculateProductTax(TaxRate taxRate, decimal productTotalPrice, decimal productDiscount, decimal productPromotionalDiscount, decimal productTaxRate)
        {
            decimal productTax;
            if (taxRate.ApplyDiscountBeforeTax)
            {
                productTax = (productTotalPrice - productDiscount - productPromotionalDiscount) * (productTaxRate / 100);
            }
            else
            {
                productTax = productTotalPrice * (productTaxRate / 100);

            }

            return productTax;
        }

        private static int CalculateTaxRate(TaxRate taxRate, Product productDetails)
        {
            return (productDetails.ProductCategoryID == (int)ProductCategoryEnum.Luxury) ? (taxRate.LuxuryTaxMultiplier * taxRate.BaseTaxRate) : taxRate.BaseTaxRate;
        }
    }
}
