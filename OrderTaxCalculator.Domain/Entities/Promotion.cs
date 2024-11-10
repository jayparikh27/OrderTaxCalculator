using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class Promotion: BaseEntity
    {
      
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsValid() => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
    }
}
