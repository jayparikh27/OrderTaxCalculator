using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTaxCalculator.Application.Core.DTOs;
using OrderTaxCalculator.Application.Core.Interfaces;

namespace OrderTaxCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCalculator : ControllerBase
    {
        private readonly IOrderCalculatorService _orderCalculatorService;
        public OrderCalculator(IOrderCalculatorService orderCalculatorService)
        {
            _orderCalculatorService = orderCalculatorService;
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderDTO orderDTO)
        {
            return Ok(_orderCalculatorService.CalculateOrderTax(orderDTO));
        }
    }
}
