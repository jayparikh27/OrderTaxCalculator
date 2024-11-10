using OrderTaxCalculator.Application.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Application.Core.Interfaces
{
    public interface IOrderCalculatorService
    {
        OrderResult CalculateOrderTax(OrderDTO orderDTO);
    }
}
