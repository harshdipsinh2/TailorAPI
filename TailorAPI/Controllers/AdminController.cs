using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.Request;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTOs.Request;
using TailorAPI.Services;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]  
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IMeasurementService _measurementService;
        private readonly IProductService _productService;
        private readonly IFabricCombinedService _fabricCombinedService;
        private readonly IOrderService _orderService;
        private readonly IDashboardService _dashboardService;
        private readonly IRoleService _roleService;


        public AdminController(IAdminService adminService, 
                               ICustomerService customerService, 
                               IMeasurementService measurementService,
                               IProductService productService, 
                               IFabricCombinedService fabricCombinedService,
                               IOrderService orderService,
                               IDashboardService dashboardService,
                               IRoleService roleService)

        {
            _dashboardService = dashboardService;
            _adminService = adminService;
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
            _fabricCombinedService = fabricCombinedService;
            _orderService = orderService;
            _roleService = roleService;
        }

        //-------------------Dashboard end points ---------------------
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(summary);
        }

        // ----------- Customer Endpoints -----------


        /// Get all customers. Available to Admin, Manager, 
        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// Get customer details by ID. Available to Admin and Manager only.
        [HttpGet("GetCustomer")]

        public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// Add a new customer. Available to Admin and Manager only.
        [HttpPost("AddCustomer")]

        public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerRequestDTO customerDto)
        {
            if (customerDto == null)
                return BadRequest("Invalid customer data");

            var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.FullName }, createdCustomer);
        }

        /// Update an existing customer. Available to Admin and Manager only.
        [HttpPut("EditCustomer")]

        public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerRequestDTO customerDto)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
            if (updatedCustomer == null) return NotFound();
            return Ok(updatedCustomer);
        }

        /// Soft delete a customer. Available to Admin and Manager only.
        [HttpDelete("DeleteCustomer")]

        public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
        {
            var result = await _customerService.SoftDeleteCustomerAsync(customerId);
            if (!result) return NotFound("Customer not found.");
            return NoContent();
        }

        // ----------- Measurement Endpoints -----------

        /// Add a measurement for a customer. Available to Admin and Manager only.
        [HttpPost("AddMeasurement")]

        public async Task<IActionResult> AddMeasurement([FromQuery] int customerId, [FromBody] MeasurementRequestDTO measurementDto)
        {
            try
            {
                var measurement = await _measurementService.AddMeasurementAsync(customerId, measurementDto);
                return Ok(measurement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Get measurement details by customer ID. Available to Admin and Manager only.
        [HttpGet("GetMeasurement")]

        public async Task<IActionResult> GetMeasurementByCustomerID([FromQuery] int customerId)
        {
            var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
            if (measurement == null) return NotFound("Measurement not found.");
            return Ok(measurement);
        }

        /// Soft delete a measurement. Available to Admin and Manager only.
        [HttpDelete("DeleteMeasurement")]
        public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
        {
            var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
            if (!result) return NotFound("Measurement not found.");

            return NoContent();
        }

        /// Get all measurements. Available to Admin and Manager only.
        [HttpGet("GetAllMeasurements")]

        public async Task<IActionResult> GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllMeasurementsAsync();
            if (measurements == null || measurements.Count == 0)
                return NotFound("No measurements found.");

            return Ok(measurements);

        }

        // ----------- Product Endpoints -----------

        /// Add a new product. Available to Admin and Manager only.
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequestDTO productDto)
        {
            var result = await _productService.AddProduct(productDto);
            return Ok(result);
        }

        /// Update an existing product by ID. Available to Admin and Manager only.
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDTO productDto)
        {
            var result = await _productService.UpdateProduct(id, productDto);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        /// Delete a product by ID. Available to Admin and Manager only.
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProduct(id);
            if (!success) return NotFound("Product not found.");
            return NoContent();
        }

        /// Get product details by ID. Available to Admin, Manager, and Tailor.
        [HttpGet("GetProduct/{id}")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductById(id);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        /// Get all products. Available to Admin, Manager, and Tailor.
        [HttpGet("GetAllProducts")]

        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

        //-------------------Fabric Endpoints ----------------------------------

        [HttpPost("AddFabricType")]
        
        public async Task<IActionResult> AddFabricType([FromBody] FabricTypeRequestDTO requestDTO)
        {
            var result = await _fabricCombinedService.AddFabricTypeAsync(requestDTO);
            return Ok(result);
        }

        [HttpPut("UpdateFabricPrice")]
        public async Task<IActionResult> UpdateFabricPrice(int id, decimal newPrice)
        {
            var result = await _fabricCombinedService.UpdateFabricTypePriceAsync(id, newPrice);
            return Ok(result);
        }

        [HttpGet("GetAllFabricTypes")]
        public async Task<IActionResult> GetAllFabricTypes()
        {
            var result = await _fabricCombinedService.GetAllFabricTypesAsync();
            return Ok(result);
        }

        [HttpGet("GetFabricTypeById")]
        public async Task<IActionResult> GetFabricTypeById(int id)
        {
            var result = await _fabricCombinedService.GetFabricTypeByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("SoftDeleteFabricType")]
        public async Task<IActionResult> SoftDeleteFabricType(int id)
        {
            var result = await _fabricCombinedService.SoftDeleteFabricTypeAsync(id);
            return Ok(result);
        }

        //---------------------------------------------------------------------------------------
        // -----------------------------FabricStock Endpoints
        [HttpPost("AddFabricStock")]
        public async Task<IActionResult> AddFabricStock([FromBody] FabricStockRequestDTO requestDTO)
        {
            var result = await _fabricCombinedService.AddFabricStockAsync(requestDTO);
            return Ok(result);
        }

        [HttpGet("GetAllFabricStocks")]
        public async Task<IActionResult> GetAllFabricStocks()
        {
            var result = await _fabricCombinedService.GetAllFabricStocksAsync();
            return Ok(result);
        }

        [HttpGet("GetFabricStockById")]
        public async Task<IActionResult> GetFabricStockById(int id)
        {
            var result = await _fabricCombinedService.GetFabricStockByIdAsync(id);
            return Ok(result);
        }

        //---------------------------------order endpoints ---------------------------------------

        [HttpPost("Create-Order")]
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
        [HttpPut("Update-Order/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, int productId, int fabricTypeId, int assignedTo, [FromBody] OrderRequestDto request)
        {
            var result = await _orderService.UpdateOrderAsync(id, productId, fabricTypeId, assignedTo, request);
            if (!result) return NotFound("Order not found.");

            return Ok("Order updated successfully.");
        }
        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto statusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, statusDto);
            if (!result)
                return NotFound("Order not found");

            return Ok("Order status updated successfully.");
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var revenue = await _orderService.GetTotalRevenueAsync();
            return Ok(revenue);
        }


        [HttpDelete("Delete-Order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);
            if (!result) return NotFound("Order not found.");

            return Ok("Order deleted successfully.");
        }

        [HttpGet("Get-Order/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpGet("GetAll-Order")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        //---------------------------------role -------------------------------------
        [HttpGet("GetAll-Role")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("GetById-Role/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound("Role not found.");
            return Ok(role);
        }

        [HttpPut("Update-Role{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string newRoleName)
        {
            var result = await _roleService.UpdateRoleAsync(id, newRoleName);
            if (!result) return NotFound("Role not found.");
            return Ok("Role updated successfully.");
        }

        [HttpDelete("Delete-Role{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound("Role not found.");
            return Ok("Role deleted successfully.");
        }
    }
}