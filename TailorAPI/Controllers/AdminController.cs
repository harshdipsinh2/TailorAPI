using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        // GET: api/Admin/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null) return NotFound("Admin not found.");
            return Ok(admin);
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRequestDto adminDto)
        {
            var result = await _adminService.RegisterAdminAsync(adminDto);
            if (!result) return BadRequest("Failed to register admin.");
            return Ok("Admin registered successfully.");
        }

        // PUT: api/Admin/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody] UserRequestDto updatedAdminDto)
        {
            var result = await _adminService.UpdateAdminAsync(id, updatedAdminDto);
            if (!result) return NotFound("Admin not found or update failed.");
            return Ok("Admin updated successfully.");
        }

        // DELETE: api/Admin/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var result = await _adminService.DeleteAdminAsync(id);
            if (!result) return NotFound("Admin not found or deletion failed.");
            return Ok("Admin deleted successfully.");
        }
    }
}
