using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class ProductCategory: BaseEntity
    {
       
        public string ProductCategoryName { get; set; }
        // Navigation property for Products
        public virtual ICollection<Product> Products { get; set; }
    }
}
