using OrderTaxCalculator.Application.Core.RepositoryInterfaces;
using OrderTaxCalculator.Application.Core.Shared.Enums;
using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Persistance.Repository
{
    internal class ProductRepository : IProductRepository
    {   // This enum created for readable purpose. Not required in the appliation
        /// <summary>
        /// This method returns List of  products with price and category.
        /// </summary>
        /// <param name="productNames">List of Product Names.</param>
        /// <returns>Returns Dictionary of Product Name and product details.</returns>
        public Dictionary<string,Product> GetProducts(List<string> productNames)
        {
            // Joint with ProductCategory table is ignored as data base is not available.
            return new List<Product>
{
                    new Product { ID = 1, Name = "Standard Chair", Price = 50.00m, ProductCategoryID = (int)ProductCategoryEnum.Standard },
                    new Product { ID = 2, Name = "Office Desk", Price = 200.00m, ProductCategoryID =(int) ProductCategoryEnum.Standard },
                    new Product { ID = 3, Name = "Luxury Sofa", Price = 1500.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                    new Product { ID = 4, Name = "Standard Lamp", Price = 30.00m, ProductCategoryID =(int) ProductCategoryEnum.Standard },
                    new Product { ID = 5, Name = "Designer Watch", Price = 2500.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                    new Product { ID = 6, Name = "Standard Bedframe", Price = 400.00m, ProductCategoryID =(int) ProductCategoryEnum.Standard },
                    new Product { ID = 7, Name = "Luxury Necklace", Price = 3000.00m, ProductCategoryID =(int) ProductCategoryEnum.Luxury },
                    new Product { ID = 8, Name = "Coffee Table", Price = 120.00m, ProductCategoryID =(int) ProductCategoryEnum.Standard }
                        }.Where(x => productNames.Contains(x.Name)).ToDictionary(x=>x.Name, x=>x);
        }
    }
}
