using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class TaxRate: BaseEntity
    {
       
        public int StateID { get; set; }
        public int BaseTaxRate { get; set; }
        public int LuxuryTaxMultiplier { get; set; }
        public bool ApplyDiscountBeforeTax { get; set; }
        public virtual State State { get; set; } // Navigation property
    }
}
