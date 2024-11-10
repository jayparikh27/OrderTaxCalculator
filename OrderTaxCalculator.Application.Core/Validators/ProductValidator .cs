using FluentValidation;
using OrderTaxCalculator.Application.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.Validators
{
    internal class ProductValidator: AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Product quantity must be greater than zero.");     
        }
    }
}
