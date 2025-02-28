using Microsoft.AspNetCore.Mvc;
using TailorAPI.Services.Interface;
using TailorAPI.Repositories;

namespace TailorAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromQuery] int customerID, [FromQuery] int productID, [FromQuery] int employeeID, [FromQuery] int quantity)
        {
            var result = await _orderService.AddOrder(customerID, productID, employeeID, quantity);
            return Ok(result);
        }

        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }
    }



}
