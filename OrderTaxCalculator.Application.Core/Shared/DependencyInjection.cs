using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Application.Core.Interfaces;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Application.Core.Services;
using OrderTaxCalculator.Application.Core.Validators;
using OrderTaxCalculator.Domain.Entities;

namespace OrderTaxCalculator.Application.Core.Shared
{
    public class DependencyInjection
    {
        public static void AddDependencyInjection(IServiceCollection services )
        {
            services.AddScoped<IOrderCalculatorService, OrderCalculatorService>();
            services.AddScoped<IValidator<ProductDTO>, ProductValidator>();
            services.AddScoped<IValidator<OrderDTO>, OrderValidator>();
        }
    }
}
