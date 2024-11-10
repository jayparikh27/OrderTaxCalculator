using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Application.Core.Interfaces;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Application.Core.Services;
using OrderTaxCalculator.Application.Core.Shared.Enums;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using Xunit;

public class OrderCalculatorTests
{
    public enum StateNameEnum
    {
        FL = 1,
        NM = 2,
        NV = 3,
        GA = 4,
        NY = 5,
    }
    [Fact]
    public void CreateOrder_WithCoupon_WithPrmotion_Tax_PreDiscount_CalculateTax()
    {
        OrderDTO orderDTO = new OrderDTO
        {
            Products = [
             new ProductDTO{
                Name = "Standard Chair",
                Quantity = 2
            }, new ProductDTO
            {
                Name = "Luxury Sofa",
                Quantity = 2
            }
                    ],
            ClientName = "Client GA",
            OrderDate = Convert.ToDateTime("2024-11-09")
        };


        var orderValidator = new Mock<IValidator<OrderDTO>>();
        var validationResult = new ValidationResult
        {
            Errors = []
        };
        orderValidator.Setup(validator => validator.Validate(It.IsAny<OrderDTO>())).Returns(validationResult);
        var clientRepository = new Mock<IClientRepository>();
        clientRepository.Setup(s => s.GetStateNameForClient(orderDTO.ClientName)).Returns(new State { ID = 1, StateName = StateNameEnum.GA.ToString() });

        var taxRateRepository = new Mock<ITaxRateRepository>();
        taxRateRepository.Setup(s => s.GetTaxRatesByState("GA"))
            .Returns(new TaxRate
            {
                ID = 4,
                StateID = (int)StateNameEnum.GA,
                BaseTaxRate = 10,
                LuxuryTaxMultiplier = 1,
                ApplyDiscountBeforeTax = false,
                State = new State { ID = 4, StateName = StateNameEnum.GA.ToString() }
            });

        var productRepository = new Mock<IProductRepository>();
        Dictionary<string, Product> dictionaryProducts = new List<Product> {
                    new Product { ID = 1, Name = "Standard Chair", Price = 50.00m, ProductCategoryID = (int)ProductCategoryEnum.Standard },
                    new Product { ID = 3, Name = "Luxury Sofa", Price = 1500.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                        }.ToDictionary(x => x.Name, x => x);
        List<string> productNames = orderDTO.Products.Select(x => x.Name).ToList();
        productRepository.Setup(s => s.GetProducts(productNames)).Returns(dictionaryProducts);

        var couponRepository = new Mock<ICouponRepository>();
        var couponDictionary = new List<Coupon>
                {
                    new Coupon { ID= 1, ProductID =1, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30), Product = new Product { Name ="Standard Chair" } },
                }
            .ToDictionary(x => x.ProductID, x => x.DiscountPercentage);

        couponRepository.Setup(x => x.GetCouponsByProductIdsAndDate(productNames, orderDTO.OrderDate)).Returns(couponDictionary);

        var promotionRepository = new Mock<IPromotionRepository>();
        var promotion = new Promotion { ID = 1, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 15) };
        promotionRepository.Setup(s => s.GetMaximumPromotionalDiscountByDate(orderDTO.OrderDate))
            .Returns(promotion);
        var orderGenericRepository = new Mock<IGenericRepository<Order>>();
        orderGenericRepository.Setup(x => x.Add(It.IsAny<Order>()));

        var mockMapper = new Mock<IMapper>();
        mockMapper
           .Setup(m => m.Map<OrderResult>(It.IsAny<OrderResult>()))
           .Returns(new OrderResult { PreTaxTotal =0});

        var orderResultGenericRepository = new Mock<IGenericRepository<OrderResult>>();
        orderResultGenericRepository.Setup(x => x.Add(It.IsAny<OrderResult>()));
    
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChanges());

        OrderCalculatorService orderCalculatorService = new OrderCalculatorService(orderValidator.Object,
            clientRepository.Object,
            taxRateRepository.Object,
            productRepository.Object,
            couponRepository.Object,
            promotionRepository.Object,
            orderGenericRepository.Object,
            mockMapper.Object,
            orderResultGenericRepository.Object,
            unitOfWork.Object);
        var result = orderCalculatorService.CalculateOrderTax(orderDTO);

 
          Assert.Equal(2935.50m,result.PreTaxTotal);
        Assert.Equal(310m, result.TaxAmount);
        Assert.Equal(3245.50m, result.TotalCost);
    }


    [Fact]
    public void CreateOrder_WithCoupon_WithPrmotion_Tax_Luxury_Multiplier_PreDiscount_CalculateTax()
    {
        OrderDTO orderDTO = new OrderDTO
        {
            Products = [
             new ProductDTO{
                Name = "Standard Chair",
                Quantity = 2
            }, new ProductDTO
            {
                Name = "Luxury Sofa",
                Quantity = 2
            }
                    ],
            ClientName = "Client GA",
            OrderDate = Convert.ToDateTime("2024-11-09")
        };


        var orderValidator = new Mock<IValidator<OrderDTO>>();
        var validationResult = new ValidationResult
        {
            Errors = []
        };
        orderValidator.Setup(validator => validator.Validate(It.IsAny<OrderDTO>())).Returns(validationResult);
        var clientRepository = new Mock<IClientRepository>();
        clientRepository.Setup(s => s.GetStateNameForClient(orderDTO.ClientName)).Returns(new State { ID = 1, StateName = StateNameEnum.GA.ToString() });

        var taxRateRepository = new Mock<ITaxRateRepository>();
        taxRateRepository.Setup(s => s.GetTaxRatesByState("GA"))
            .Returns(new TaxRate
            {
                ID = 4,
                StateID = (int)StateNameEnum.GA,
                BaseTaxRate = 10,
                LuxuryTaxMultiplier = 2,
                ApplyDiscountBeforeTax = false,
                State = new State { ID = 4, StateName = StateNameEnum.GA.ToString() }
            });

        var productRepository = new Mock<IProductRepository>();
        Dictionary<string, Product> dictionaryProducts = new List<Product> {
                    new Product { ID = 1, Name = "Standard Chair", Price = 50.00m, ProductCategoryID = (int)ProductCategoryEnum.Standard },
                    new Product { ID = 3, Name = "Luxury Sofa", Price = 1500.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                        }.ToDictionary(x => x.Name, x => x);
        List<string> productNames = orderDTO.Products.Select(x => x.Name).ToList();
        productRepository.Setup(s => s.GetProducts(productNames)).Returns(dictionaryProducts);

        var couponRepository = new Mock<ICouponRepository>();
        var couponDictionary = new List<Coupon>
                {
                    new Coupon { ID= 1, ProductID =1, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30), Product = new Product { Name ="Standard Chair" } },
                }
            .ToDictionary(x => x.ProductID, x => x.DiscountPercentage);

        couponRepository.Setup(x => x.GetCouponsByProductIdsAndDate(productNames, orderDTO.OrderDate)).Returns(couponDictionary);

        var promotionRepository = new Mock<IPromotionRepository>();
        var promotion = new Promotion { ID = 1, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 15) };
        promotionRepository.Setup(s => s.GetMaximumPromotionalDiscountByDate(orderDTO.OrderDate))
            .Returns(promotion);
        var orderGenericRepository = new Mock<IGenericRepository<Order>>();
        orderGenericRepository.Setup(x => x.Add(It.IsAny<Order>()));

        var mockMapper = new Mock<IMapper>();
        mockMapper
           .Setup(m => m.Map<OrderResult>(It.IsAny<OrderResult>()))
           .Returns(new OrderResult { PreTaxTotal = 0 });

        var orderResultGenericRepository = new Mock<IGenericRepository<OrderResult>>();
        orderResultGenericRepository.Setup(x => x.Add(It.IsAny<OrderResult>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChanges());

        OrderCalculatorService orderCalculatorService = new OrderCalculatorService(orderValidator.Object,
            clientRepository.Object,
            taxRateRepository.Object,
            productRepository.Object,
            couponRepository.Object,
            promotionRepository.Object,
            orderGenericRepository.Object,
            mockMapper.Object,
            orderResultGenericRepository.Object,
            unitOfWork.Object);
        var result = orderCalculatorService.CalculateOrderTax(orderDTO);


        Assert.Equal(2935.50m, result.PreTaxTotal);
        Assert.Equal(610m, result.TaxAmount);
        Assert.Equal(3545.50m, result.TotalCost);
    }

    [Fact]
    public void CreateOrder_WithCoupon_WithPrmotion_Luxury_Multiplier_PostDiscount_CalculateTax()
    {
        OrderDTO orderDTO = new OrderDTO
        {
            Products = [
             new ProductDTO{
                Name = "Standard Chair",
                Quantity = 2
            }, new ProductDTO
            {
                Name = "Luxury Sofa",
                Quantity = 2
            }
                    ],
            ClientName = "Client GA",
            OrderDate = Convert.ToDateTime("2024-11-09")
        };


        var orderValidator = new Mock<IValidator<OrderDTO>>();
        var validationResult = new ValidationResult
        {
            Errors = []
        };
        orderValidator.Setup(validator => validator.Validate(It.IsAny<OrderDTO>())).Returns(validationResult);
        var clientRepository = new Mock<IClientRepository>();
        clientRepository.Setup(s => s.GetStateNameForClient(orderDTO.ClientName)).Returns(new State { ID = 1, StateName = StateNameEnum.GA.ToString() });

        var taxRateRepository = new Mock<ITaxRateRepository>();
        taxRateRepository.Setup(s => s.GetTaxRatesByState("GA"))
            .Returns(new TaxRate
            {
                ID = 4,
                StateID = (int)StateNameEnum.GA,
                BaseTaxRate = 10,
                LuxuryTaxMultiplier = 2,
                ApplyDiscountBeforeTax = true,
                State = new State { ID = 4, StateName = StateNameEnum.GA.ToString() }
            });

        var productRepository = new Mock<IProductRepository>();
        Dictionary<string, Product> dictionaryProducts = new List<Product> {
                    new Product { ID = 1, Name = "Standard Chair", Price = 50.00m, ProductCategoryID = (int)ProductCategoryEnum.Standard },
                    new Product { ID = 3, Name = "Luxury Sofa", Price = 1500.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                        }.ToDictionary(x => x.Name, x => x);
        List<string> productNames = orderDTO.Products.Select(x => x.Name).ToList();
        productRepository.Setup(s => s.GetProducts(productNames)).Returns(dictionaryProducts);

        var couponRepository = new Mock<ICouponRepository>();
        var couponDictionary = new List<Coupon>
                {
                    new Coupon { ID= 1, ProductID =1, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30), Product = new Product { Name ="Standard Chair" } },
                }
            .ToDictionary(x => x.ProductID, x => x.DiscountPercentage);

        couponRepository.Setup(x => x.GetCouponsByProductIdsAndDate(productNames, orderDTO.OrderDate)).Returns(couponDictionary);

        var promotionRepository = new Mock<IPromotionRepository>();
        var promotion = new Promotion { ID = 1, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 15) };
        promotionRepository.Setup(s => s.GetMaximumPromotionalDiscountByDate(orderDTO.OrderDate))
            .Returns(promotion);
        var orderGenericRepository = new Mock<IGenericRepository<Order>>();
        orderGenericRepository.Setup(x => x.Add(It.IsAny<Order>()));

        var mockMapper = new Mock<IMapper>();
        mockMapper
           .Setup(m => m.Map<OrderResult>(It.IsAny<OrderResult>()))
           .Returns(new OrderResult { PreTaxTotal = 0 });

        var orderResultGenericRepository = new Mock<IGenericRepository<OrderResult>>();
        orderResultGenericRepository.Setup(x => x.Add(It.IsAny<OrderResult>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChanges());

        OrderCalculatorService orderCalculatorService = new OrderCalculatorService(orderValidator.Object,
            clientRepository.Object,
            taxRateRepository.Object,
            productRepository.Object,
            couponRepository.Object,
            promotionRepository.Object,
            orderGenericRepository.Object,
            mockMapper.Object,
            orderResultGenericRepository.Object,
            unitOfWork.Object);
        var result = orderCalculatorService.CalculateOrderTax(orderDTO);


        Assert.Equal(2935.50m, result.PreTaxTotal);
        Assert.Equal(578.55m, result.TaxAmount);
        Assert.Equal(3514.05m, result.TotalCost);
    }

    [Fact]
    public void CreateOrder_With_InvalidProduct()
    {
        OrderDTO orderDTO = new OrderDTO
        {
            Products = [
             new ProductDTO{
                Name = "Standard Chair",
                Quantity = 2
            }, new ProductDTO
            {
                Name = "Luxury Sofa",
                Quantity = 2
            }
                    ],
            ClientName = "Client GA",
            OrderDate = Convert.ToDateTime("2024-11-09")
        };


        var orderValidator = new Mock<IValidator<OrderDTO>>();
        var validationResult = new ValidationResult
        {
            Errors = []
        };
        orderValidator.Setup(validator => validator.Validate(It.IsAny<OrderDTO>())).Returns(validationResult);
        var clientRepository = new Mock<IClientRepository>();
        clientRepository.Setup(s => s.GetStateNameForClient(orderDTO.ClientName)).Returns(new State { ID = 1, StateName = StateNameEnum.GA.ToString() });

        var taxRateRepository = new Mock<ITaxRateRepository>();
        taxRateRepository.Setup(s => s.GetTaxRatesByState("GA"))
            .Returns(new TaxRate
            {
                ID = 4,
                StateID = (int)StateNameEnum.GA,
                BaseTaxRate = 10,
                LuxuryTaxMultiplier = 2,
                ApplyDiscountBeforeTax = true,
                State = new State { ID = 4, StateName = StateNameEnum.GA.ToString() }
            });

        var productRepository = new Mock<IProductRepository>();
        Dictionary<string, Product> dictionaryProducts = new List<Product> {
                    new Product { ID = 1, Name = "Standard Chair", Price = 50.00m, ProductCategoryID = (int)ProductCategoryEnum.Standard },
                   
                        }.ToDictionary(x => x.Name, x => x);
        List<string> productNames = orderDTO.Products.Select(x => x.Name).ToList();
        productRepository.Setup(s => s.GetProducts(productNames)).Returns(dictionaryProducts);

        var couponRepository = new Mock<ICouponRepository>();
        var couponDictionary = new List<Coupon>
                {
                    new Coupon { ID= 1, ProductID =1, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30), Product = new Product { Name ="Standard Chair" } },
                }
            .ToDictionary(x => x.ProductID, x => x.DiscountPercentage);

        couponRepository.Setup(x => x.GetCouponsByProductIdsAndDate(productNames, orderDTO.OrderDate)).Returns(couponDictionary);

        var promotionRepository = new Mock<IPromotionRepository>();
        var promotion = new Promotion { ID = 1, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 15) };
        promotionRepository.Setup(s => s.GetMaximumPromotionalDiscountByDate(orderDTO.OrderDate))
            .Returns(promotion);
        var orderGenericRepository = new Mock<IGenericRepository<Order>>();
        orderGenericRepository.Setup(x => x.Add(It.IsAny<Order>()));

        var mockMapper = new Mock<IMapper>();
        mockMapper
           .Setup(m => m.Map<OrderResult>(It.IsAny<OrderResult>()))
           .Returns(new OrderResult { PreTaxTotal = 0 });

        var orderResultGenericRepository = new Mock<IGenericRepository<OrderResult>>();
        orderResultGenericRepository.Setup(x => x.Add(It.IsAny<OrderResult>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChanges());

        OrderCalculatorService orderCalculatorService = new OrderCalculatorService(orderValidator.Object,
            clientRepository.Object,
            taxRateRepository.Object,
            productRepository.Object,
            couponRepository.Object,
            promotionRepository.Object,
            orderGenericRepository.Object,
            mockMapper.Object,
            orderResultGenericRepository.Object,
            unitOfWork.Object);


        var exception = Assert.Throws<Exception>(() => orderCalculatorService.CalculateOrderTax(orderDTO));
        Assert.Equal("Invalid products", exception.Message);
    }
}