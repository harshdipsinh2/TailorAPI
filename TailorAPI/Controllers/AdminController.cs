using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.DTO.Request;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTOs.Request;
using TailorAPI.Repositories;
using TailorAPI.Services;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;
        private readonly IOtpVerificationService _otpVerificationService;
        private readonly IAuthService _authService;
        private readonly UserRepository _userRepository;


        public AdminController(IAdminService adminService,
                                UserRepository userRepository,
                                IOtpVerificationService otpVerificationService,
                               ICustomerService customerService,
                               IMeasurementService measurementService,
                               IProductService productService,
                               IFabricCombinedService fabricCombinedService,
                               IOrderService orderService,
                               IDashboardService dashboardService,
                                 IBranchService branchService,
                                 IAuthService authService,
                                 IUserService userService,
                               IRoleService roleService)
        {
            _dashboardService = dashboardService;
            _userService = userService;
            _otpVerificationService = otpVerificationService;
            _userRepository = userRepository;
            _authService = authService;
            _adminService = adminService;
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
            _fabricCombinedService = fabricCombinedService;
            _orderService = orderService;
            _branchService = branchService;
            _roleService = roleService;
        }

        //-------------------Dashboard end points ---------------------

        [HttpGet("summary")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(summary);
        }

        // ----------- Customer Endpoints -----------




        //[HttpGet("GetAllCustomers")]
        //[Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        //public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        //{
        //    var customers = await _customerService.GetAllCustomersAsync();
        //    return Ok(customers);
        //}
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllCustomer-Admin")]
        public async Task<IActionResult> GetCustomersForAdmin([FromQuery] int? shopId, [FromQuery] int? branchId)
        {
            try
            {
                var customers = await _customerService.GetCustomersForAdminAsync(shopId, branchId);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles ="Manager")]
        [HttpGet("GetAllCustomer-Manager")]
        public async Task<IActionResult> GetCustomersForManager()
        {
            try
            {
                var customers = await _customerService.GetCustomersForManagerAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("GetAllCustomersForSuperAdmin")]
        public async Task<IActionResult> GetAllCustomersForSuperAdmin([FromQuery] int shopId, [FromQuery] int? branchId = null)
        {
            var customers = await _customerService.GetAllCustomersForSuperAdminAsync(shopId, branchId);
            return Ok(customers);
        }

        [HttpGet("GetCustomer")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost("AddCustomer")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]

        public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerRequestDTO customerDto)
        {
            if (customerDto == null)
                return BadRequest("Invalid customer data");

            try
            {
                var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
                return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.CustomerId }, createdCustomer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("EditCustomer")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerRequestDTO customerDto)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
            if (updatedCustomer == null) return NotFound();
            return Ok(updatedCustomer);
        }

        [HttpDelete("DeleteCustomer")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
        {
            var result = await _customerService.SoftDeleteCustomerAsync(customerId);
            if (!result) return NotFound("Customer not found.");
            return NoContent();
        }

        // ----------- Measurement Endpoints -----------

        [HttpPost("AddMeasurement")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
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

        [HttpGet("GetAllMeasurementForSuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllMeasurementsForSuperAdmin()
        {
            var measurements = await _measurementService.GetallMeasurementForSuperAdminAsync();
            return Ok(measurements);
        }

        [HttpGet("GetMeasurement")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetMeasurementByCustomerID([FromQuery] int customerId)
        {
            var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
            if (measurement == null) return NotFound("Measurement not found.");
            return Ok(measurement);
        }

        [HttpDelete("DeleteMeasurement")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
        {
            var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
            if (!result) return NotFound("Measurement not found.");

            return NoContent();
        }



        [HttpGet("GetAllMeasurements")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllMeasurementsAsync();
            if (measurements == null || measurements.Count == 0)
                return NotFound("No measurements found.");

            return Ok(measurements);
        }

        // ----------- Product Endpoints -----------

        [HttpPost("AddProduct")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequestDTO productDto)

        {

            if (productDto == null)
                return BadRequest("Invalid product data");
            try
            {
                var result = await _productService.AddProduct(productDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDTO productDto)
        {
            var result = await _productService.UpdateProduct(id, productDto);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProduct(id);
            if (!success) return NotFound("Product not found.");
            return NoContent();
        }

        [HttpGet("GetProduct/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductById(id);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }
        [HttpGet("GetAllProductsForSuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllProductsForSuperAdmin()
        {
            var result = await _productService.GetAllProductsForSuperAdmin();
            return Ok(result);
        }

        //-------------------Fabric Endpoints ----------------------------------

        [HttpPost("AddFabricType")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> AddFabricType([FromBody] FabricTypeRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest("Invalid fabric type data");
            }try

            { var result = await _fabricCombinedService.AddFabricTypeAsync(requestDTO);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateFabricPrice")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> UpdateFabricPrice(int id, decimal newPrice)
        {
            var result = await _fabricCombinedService.UpdateFabricTypePriceAsync(id, newPrice);
            return Ok(result);
        }

        [HttpGet("GetAllFabricTypeForSuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllFabricTypeForSuperAdmin()
        {
            var result = await _fabricCombinedService.GetAllFabricTypeForSuperAdmin();
            return Ok(result);
        }



        [HttpGet("GetAllFabricTypes")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetAllFabricTypes()
        {
            var result = await _fabricCombinedService.GetAllFabricTypesAsync();
            return Ok(result);
        }

        [HttpGet("GetFabricTypeById")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetFabricTypeById(int id)
        {
            var result = await _fabricCombinedService.GetFabricTypeByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("SoftDeleteFabricType")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> SoftDeleteFabricType(int id)
        {
            var result = await _fabricCombinedService.SoftDeleteFabricTypeAsync(id);
            return Ok(result);
        }

        //---------------------------------------------------------------------------------------
        // -----------------------------FabricStock Endpoints
        [HttpPost("AddFabricStock")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> AddFabricStock([FromBody] FabricStockRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest("Invalid fabric stock data");

            }
            try
            { 
                var result = await _fabricCombinedService.AddFabricStockAsync(requestDTO);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllFabricStocks")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetAllFabricStocks()
        {
            var result = await _fabricCombinedService.GetAllFabricStocksAsync();
            return Ok(result);
        }

        [HttpGet("GetFabricStockById")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetFabricStockById(int id)
        {
            var result = await _fabricCombinedService.GetFabricStockByIdAsync(id);
            return Ok(result);
        }

        //---------------------------------order endpoints ---------------------------------------

        [HttpPost("Create-Order")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
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
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> UpdateOrder(int id, int productId, int fabricTypeId, int assignedTo, [FromBody] OrderRequestDto request)
        {
            var result = await _orderService.UpdateOrderAsync(id, productId, fabricTypeId, assignedTo, request);
            if (!result) return NotFound("Order not found.");

            return Ok("Order updated successfully.");
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto statusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, statusDto);
            if (!result)
                return NotFound("Order not found");

            return Ok("Order status updated successfully.");
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var revenue = await _orderService.GetTotalRevenueAsync();
            return Ok(revenue);
        }

        [HttpDelete("Delete-Order/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);
            if (!result) return NotFound("Order not found.");

            return Ok("Order deleted successfully.");
        }

        [HttpGet("Get-Order/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpPut("{orderId}/approval")]
        [Authorize(Roles = "SuperAdmin,Tailor")]
        public async Task<IActionResult> UpdateOrderApproval(int orderId, [FromBody] OrderApprovalUpdateDTO requestDto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim missing");
                }

                int UserID = int.Parse(userIdClaim.Value);

                var result = await _orderService.UpdateOrderApprovalAsync(orderId, UserID, requestDto);
                if (!result) return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("GetAll-Order")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetAllOrders()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID" || c.Type == ClaimTypes.NameIdentifier);
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                return Unauthorized("Missing claim information.");

            int userId = int.Parse(userIdClaim.Value);
            string role = roleClaim.Value;

            var orders = await _orderService.GetAllOrdersAsync(userId, role);
            return Ok(orders);
        }

        [HttpGet("rejected")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager,Tailor")]
        public async Task<IActionResult> GetRejectedOrders()
        {
            var rejectedOrders = await _orderService.GetRejectedOrdersAsync();
            return Ok(rejectedOrders);
        }

        [HttpPost("orders/{orderId}/reassign")]
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        public async Task<IActionResult> ReassignRejectedOrder(int orderId, ReassignOrderDTO dto)
        {
            var result = await _orderService.ReassignRejectedOrderAsync(orderId, dto);
            if (!result) return BadRequest("Order cannot be reassigned.");
            return Ok("Order reassigned successfully.");
        }

        //---------------------------------role -------------------------------------
        [HttpGet("GetAll-Role")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("GetById-Role/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound("Role not found.");
            return Ok(role);
        }

        [HttpPut("Update-Role{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string newRoleName)
        {
            var result = await _roleService.UpdateRoleAsync(id, newRoleName);
            if (!result) return NotFound("Role not found.");
            return Ok("Role updated successfully.");
        }

        [HttpDelete("Delete-Role{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound("Role not found.");
            return Ok("Role deleted successfully.");
        }

        // -----------------------------Branch Endpoints ---------------------------------------

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("create-Branch")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchRequestDTO dto)
        {
            try
            {
                var branch = await _branchService.CreateBranchAsync(dto);
                return Ok(branch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("all-branches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var result = await _branchService.GetAllBranchesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("All-BranchesForSuperAdmin")]
        public async Task<IActionResult> GetAllBranchesForSuperAdmin()
        {
            var result = await _branchService.GetAllBranchForSuperAdmin();
            return Ok(result);
        }



        //----------------------------------sUser endpoints
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpGet("GetAll-User")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles ="SuperAdmin,Admin")]
        [HttpGet("admin/users")]
        public async Task<IActionResult> GetUsersByShop([FromQuery] int shopId, [FromQuery] int? branchId = null)
        {
            var users = await _userService.GetUsersByShopAsync(shopId, branchId);
            return Ok(users);
        }

        [Authorize(Roles ="Manager")]
        [HttpGet("manager/users")]
        public async Task<IActionResult> GetUsersByBranch([FromQuery] int shopId, [FromQuery] int branchId)
        {
            var users = await _userService.GetUsersByBranchAsync(shopId, branchId);
            return Ok(users);
        }


        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpGet("GetAllTailor-User")]
        public async Task<IActionResult> GetAllTailors()
        {
            var tailors = await _userService.GetAllTailorsAsync();
            return Ok(tailors);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpGet("GetUserById/{id}")]

        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            return Ok(user);
        }
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpDelete("Delete-User/{id}")]

        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found.");

            return Ok("User deleted successfully.");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpPut("Update-User/{id}")]

        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            if (updatedUser == null) return NotFound("User not found.");

            return Ok(updatedUser);
        }

        [Authorize(Roles ="SuperAdmin,Admin")]
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegistrationDto request)
        {
            return await _authService.RegisterEmployeeAsync(request);
        }
    }
}

