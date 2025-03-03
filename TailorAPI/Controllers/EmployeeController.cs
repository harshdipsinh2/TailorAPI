//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using TailorAPI.DTO;

//[Route("api/[controller]")]
//[ApiController]
//public class EmployeeController : ControllerBase
//{
//    private readonly IEmployeeService _employeeService;

//    public EmployeeController(IEmployeeService employeeService)
//    {
//        _employeeService = employeeService;
//    }

//    // ✅ 1. Get all employees
//    [HttpGet("GetAllEmployees")]
//    public async Task<ActionResult<List<EmployeeDTO>>> GetAllEmployees()
//    {
//        var employees = await _employeeService.GetAllEmployeesAsync();
//        return Ok(employees);
//    }

//    // ✅ 2. Get employee by ID
//    [HttpGet("GetEmployee")]
//    public async Task<ActionResult<EmployeeDTO>> GetEmployeeById([FromQuery] int employeeId)
//    {
//        var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
//        if (employee == null) return NotFound();
//        return Ok(employee);
//    }

//    // ✅ 3. Add new employee
//    [HttpPost("AddEmployee")]
//    public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody] EmployeeDTO employeeDto)
//    {
//        if (employeeDto == null)
//            return BadRequest("Invalid employee data");

//        var createdEmployee = await _employeeService.AddEmployeeAsync(employeeDto);
//        return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = createdEmployee.FullName }, createdEmployee);
//    }

//    // ✅ 4. Update employee
//    [HttpPut("UpdateEmployee")]
//    public async Task<IActionResult> UpdateEmployee([FromQuery] int employeeId, [FromBody] EmployeeDTO employeeDto)
//    {
//        var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeId, employeeDto);
//        if (updatedEmployee == null) return NotFound();
//        return Ok(updatedEmployee);
//    }

//    // ✅ 5. Soft delete employee
//    [HttpDelete("DeleteEmployee")]
//    public async Task<IActionResult> SoftDeleteEmployee([FromQuery] int employeeId)
//    {
//        var result = await _employeeService.SoftDeleteEmployeeAsync(employeeId);
//        if (!result) return NotFound("Employee not found.");
//        return NoContent();
//    }
//}
