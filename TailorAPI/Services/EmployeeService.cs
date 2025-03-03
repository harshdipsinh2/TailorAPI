//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TailorAPI.DTO;
//using TailorAPI.Models;
//using TailorAPI.Repositories;

//public class EmployeeService : IEmployeeService
//{
//    private readonly EmployeeRepository _employeeRepository;
//    private readonly UserManager<AppUser> _userManager;
//    private readonly RoleManager<IdentityRole> _roleManager;

//    public EmployeeService(EmployeeRepository employeeRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
//    {
//        _employeeRepository = employeeRepository;
//        _userManager = userManager;
//        _roleManager = roleManager;
//    }

//    public async Task<EmployeeDTO?> AddEmployeeAsync(EmployeeDTO employeeDto)
//    {
//        // Step 1: Validate Required Fields
//        if (string.IsNullOrEmpty(employeeDto.Email) || string.IsNullOrEmpty(employeeDto.FullName) || string.IsNullOrEmpty(employeeDto.Role))
//        {
//            throw new ArgumentException("FullName, Email, and Role are required.");
//        }

//        // Step 2: Check if User Already Exists
//        var existingUser = await _userManager.FindByEmailAsync(employeeDto.Email);
//        if (existingUser != null)
//        {
//            throw new InvalidOperationException("User with this email already exists.");
//        }

//        // Step 3: Create Identity User
//        var identityUser = new AppUser
//        {
//            UserName = employeeDto.Email,
//            Email = employeeDto.Email
//        };

//        var result = await _userManager.CreateAsync(identityUser, "Default@123"); // Default password

//        if (!result.Succeeded)
//        {
//            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
//        }

//        // Step 4: Ensure Role Exists Before Assigning
//        if (!await _roleManager.RoleExistsAsync(employeeDto.Role))
//        {
//            await _roleManager.CreateAsync(new IdentityRole(employeeDto.Role));
//        }

//        await _userManager.AddToRoleAsync(identityUser, employeeDto.Role);

//        // Step 5: Create Employee Entry in Database
//        var newEmployee = new Employee
//        {
//            FullName = employeeDto.FullName,
//            PhoneNumber = employeeDto.PhoneNumber ?? string.Empty, // Ensure non-null values
//            Email = employeeDto.Email,
//            Address = employeeDto.Address ?? string.Empty,
//            Role = employeeDto.Role,
//            Salary = employeeDto.Salary,
//            Attendance = employeeDto.Attendance,
//            Status = employeeDto.Status,
//            CustomerID = employeeDto.CustomerID,
//            MeasurementID = employeeDto.MeasurementID,
//            IdentityUserId = identityUser.Id  // Linking Employee to Identity User
//        };

//        var createdEmployee = await _employeeRepository.AddAsync(newEmployee);

//        return new EmployeeDTO
//        {
//            FullName = createdEmployee.FullName,
//            PhoneNumber = createdEmployee.PhoneNumber,
//            Email = createdEmployee.Email,
//            Address = createdEmployee.Address,
//            Role = createdEmployee.Role,
//            Salary = createdEmployee.Salary,
//            Attendance = createdEmployee.Attendance,
//            Status = createdEmployee.Status,
//            CustomerID = createdEmployee.CustomerID,
//            MeasurementID = createdEmployee.MeasurementID
//        };
//    }

//    public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
//    {
//        var employees = await _employeeRepository.GetAllAsync();

//        return employees.Select(e => new EmployeeDTO
//        {
//            FullName = e.FullName,
//            PhoneNumber = e.PhoneNumber,
//            Email = e.Email,
//            Address = e.Address,
//            Role = e.Role,
//            Salary = e.Salary,
//            Attendance = e.Attendance,
//            Status = e.Status,
//            CustomerID = e.CustomerID,
//            MeasurementID = e.MeasurementID
//        }).ToList();
//    }

//    public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int employeeId)
//    {
//        var employee = await _employeeRepository.GetByIdAsync(employeeId);
//        if (employee == null) return null;

//        return new EmployeeDTO
//        {
//            FullName = employee.FullName,
//            PhoneNumber = employee.PhoneNumber,
//            Email = employee.Email,
//            Address = employee.Address,
//            Role = employee.Role,
//            Salary = employee.Salary,
//            Attendance = employee.Attendance,
//            Status = employee.Status,
//            CustomerID = employee.CustomerID,
//            MeasurementID = employee.MeasurementID
//        };
//    }

//    public async Task<EmployeeDTO?> UpdateEmployeeAsync(int employeeId, EmployeeDTO employeeDto)
//    {
//        var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
//        if (existingEmployee == null) return null;

//        existingEmployee.FullName = employeeDto.FullName;
//        existingEmployee.PhoneNumber = employeeDto.PhoneNumber;
//        existingEmployee.Email = employeeDto.Email;
//        existingEmployee.Address = employeeDto.Address;
//        existingEmployee.Role = employeeDto.Role;
//        existingEmployee.Salary = employeeDto.Salary;
//        existingEmployee.Attendance = employeeDto.Attendance;
//        existingEmployee.Status = employeeDto.Status;
//        existingEmployee.CustomerID = employeeDto.CustomerID;
//        existingEmployee.MeasurementID = employeeDto.MeasurementID;

//        await _employeeRepository.UpdateAsync(existingEmployee);

//        return new EmployeeDTO
//        {
//            FullName = existingEmployee.FullName,
//            PhoneNumber = existingEmployee.PhoneNumber,
//            Email = existingEmployee.Email,
//            Address = existingEmployee.Address,
//            Role = existingEmployee.Role,
//            Salary = existingEmployee.Salary,
//            Attendance = existingEmployee.Attendance,
//            Status = existingEmployee.Status,
//            CustomerID = existingEmployee.CustomerID,
//            MeasurementID = existingEmployee.MeasurementID
//        };
//    }

//    public async Task<bool> SoftDeleteEmployeeAsync(int employeeId)
//    {
//        var employee = await _employeeRepository.GetByIdAsync(employeeId);
//        if (employee == null) return false;

//        employee.IsDeleted = true;
//        await _employeeRepository.UpdateAsync(employee);
//        return true;
//    }
//}
