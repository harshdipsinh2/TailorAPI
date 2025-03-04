using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders() => Ok(await _orderService.GetAllOrders());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderResponseDto request)
    {
        var order = await _orderService.CreateOrder(request);
        return Ok(order);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDto request)
    {
        var order = await _orderService.UpdateOrder(id, request.Quantity, "Updated", "Updated", DateTime.UtcNow);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var deleted = await _orderService.DeleteOrder(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

