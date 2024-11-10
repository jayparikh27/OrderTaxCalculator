using OrderTaxCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.DTOs
{
    public class OrderDTO
    {
        public List<ProductDTO> Products { get; set; } 
        public string ClientName { get; set; }
        public DateTime OrderDate {  get; set; }
    }
}
