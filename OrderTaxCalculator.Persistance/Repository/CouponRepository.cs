using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance.Repository
    { /// <summary>
      /// This method returns dictionary of productID and discount for each products.
      /// </summary>
      /// <param name="productNames">List of Product Names</param>
      ///   /// <param name="orderDate">Order Date</param>
      /// <returns>Returns Valid coupons Dictionary for each products in the order.</returns>
    internal class CouponRepository : ICouponRepository
    {

        // Join with ProductCategory, Product table is ignored as data base is not available.
        public Dictionary<int,decimal> GetCouponsByProductIdsAndDate(List<string> productNames, DateTime orderDate)
        {
            return new List<Coupon>
                {
                    new Coupon { ID= 1, ProductID =1, DiscountPercentage = 10, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30), Product = new Product { Name ="Standard Chair" } },
                    new Coupon { ID = 2, ProductID =2, DiscountPercentage = 15, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30) , Product = new Product { Name ="Office Desk" } },
                    new Coupon { ID = 3, ProductID = 3, DiscountPercentage = 20, StartDate = new DateTime(2024, 12, 1), EndDate = new DateTime(2024, 12, 31), Product = new Product { Name ="Luxury Sofa" } },
                    new Coupon { ID = 4, ProductID = 4, DiscountPercentage = 25, StartDate = new DateTime(2024, 11, 15), EndDate = new DateTime(2024, 12, 15), Product = new Product { Name = "Standard Lamp" } },
                    new Coupon { ID = 5, ProductID = 5, DiscountPercentage = 5, StartDate = new DateTime(2024, 11, 1), EndDate = new DateTime(2024, 11, 30) , Product = new Product { Name ="Designer Watch" } },
                }
            .Where(x => productNames.Contains(x.Product.Name) && x.StartDate.Date <= orderDate.Date && x.EndDate.Date >= orderDate.Date)
            .ToDictionary(x => x.ProductID, x => x.DiscountPercentage);
        }
    }
}
