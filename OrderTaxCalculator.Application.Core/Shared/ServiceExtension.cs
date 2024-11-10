using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.Shared
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            DependencyInjection.AddDependencyInjection(services);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
