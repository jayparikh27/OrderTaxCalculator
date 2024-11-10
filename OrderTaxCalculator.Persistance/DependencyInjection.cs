using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
using OrderTaxCalculator.Persistance.Repository;
namespace OrderTaxCalculator.Persistance
{
    public class DependencyInjection
    {
        public static void AddDependencyInjection(IServiceCollection services )
        {
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<ITaxRateRepository, TaxRateRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
