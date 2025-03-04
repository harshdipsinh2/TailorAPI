using Microsoft.AspNetCore.Mvc;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromQuery] int productID, [FromQuery] string productName, [FromQuery] decimal price)
        {
            var result = await _productService.AddProduct(productID, productName, price);
            return Ok(result);
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (!result) return NotFound("Product not found or already deleted.");
            return Ok("Product deleted successfully.");
        }

    }
}
