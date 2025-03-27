using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(int customerId, int productId, int fabricTypeId, int assignedTo, [FromBody] OrderRequestDto request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(customerId, productId, fabricTypeId, assignedTo, request);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, int productId, int fabricTypeId, int assignedTo, [FromBody] OrderRequestDto request)
        {
            var result = await _orderService.UpdateOrderAsync(id, productId, fabricTypeId, assignedTo, request);
            if (!result) return NotFound("Order not found.");

            return Ok("Order updated successfully.");
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);
            if (!result) return NotFound("Order not found.");

            return Ok("Order deleted successfully.");
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}