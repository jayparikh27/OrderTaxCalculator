using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class State: BaseEntity
    {
      
        public string StateName { get; set; }

        // Navigation property for Client
        public virtual ICollection<Client> Clients { get; set; }

        // Navigation property for TaxRate
        public virtual ICollection<TaxRate> TaxRates { get; set; }
    }
}
