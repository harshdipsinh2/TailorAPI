using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTOs.Request;
using TailorAPI.Services;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(
            [FromQuery] int customerId,
            [FromQuery] int productId,
            [FromQuery] int fabricId,
            [FromBody] OrderRequestDto request)
        {
            var result = await _orderService.CreateOrderAsync(customerId, productId, fabricId, request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetOrderById([FromQuery] int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrder(
            [FromQuery] int id,
            [FromQuery] int productId,
            [FromQuery] int fabricId,
            [FromBody] OrderRequestDto request)
        {
            var result = await _orderService.UpdateOrderAsync(id, productId, fabricId, request);
            if (!result) return NotFound();
            return Ok("Order updated successfully");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> SoftDeleteOrder([FromQuery] int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);
            if (!result) return NotFound();
            return Ok("Order deleted successfully");
        }
    }
}