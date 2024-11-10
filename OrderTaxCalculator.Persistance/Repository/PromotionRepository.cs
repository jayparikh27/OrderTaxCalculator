using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance.Repository
{
    internal class PromotionRepository : IPromotionRepository
    {
        /// <summary>
        /// This method returns valid promotion based on order date.
        /// </summary>
        /// <param name="orderDate">order date.</param>
        /// <returns>Returns valid promotion.</returns>
        public Promotion? GetMaximumPromotionalDiscountByDate(DateTime orderDate)
        {
            Promotion? promotion = new List<Promotion>
            {
                new Promotion { ID = 1, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 15) },
                new Promotion { ID = 2, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 20), EndDate = new DateTime(2024, 11, 30) },
                new Promotion { ID = 3, DiscountPercentage = 15, StartDate = new DateTime(2024, 12, 1), EndDate = new DateTime(2024, 12, 10) }
            }.Where(x => x.StartDate.Date <= orderDate.Date && x.EndDate.Date >= orderDate.Date).OrderByDescending(x => x.DiscountPercentage).FirstOrDefault();


            return promotion;
        }
    }
}
