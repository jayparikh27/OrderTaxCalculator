using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class Product: BaseEntity
    {
      
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryID { get; set; }
        // Navigation property for ProductCategory
         public virtual ProductCategory ProductCategory { get; set; }

        // Navigation property for Coupons
        public virtual ICollection<Coupon> Coupons { get; set; }
    }
}
