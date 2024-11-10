using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class Client:BaseEntity
    {
       
        public string ClientName { get; set; }
        public int StateID { get; set; }  // Foreign Key to State table
        public virtual State State { get; set; }  // Navigation property
    }
}
