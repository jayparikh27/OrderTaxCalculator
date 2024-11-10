using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class OrderResult:BaseEntity
    {
        public decimal PreTaxTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalCost { get; set; }
    }
}
