using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            DependencyInjection.AddDependencyInjection(services);
        }
    }
}
