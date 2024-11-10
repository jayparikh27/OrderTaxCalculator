using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class Order:BaseEntity
    {
        public int ID { get; set; }
        // I have not created order class. This will be created as per DB design.
    }
}
