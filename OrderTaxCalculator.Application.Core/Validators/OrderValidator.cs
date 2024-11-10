using FluentValidation;
using OrderTaxCalculator.Application.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.Validators
{
    internal class OrderValidator : AbstractValidator<OrderDTO>
    {
        public OrderValidator()
        {
            RuleFor(o => o.Products)
                .NotEmpty().WithMessage("Order must contain at least one product.");

            RuleFor(o => o.ClientName)
                .NotEmpty().WithMessage("ClientName is required.");
            
            RuleForEach(o => o.Products).SetValidator(new ProductValidator());
        }
    }
}
