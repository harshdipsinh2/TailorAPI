//namespace TailorAPI.Repositories;

//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//public class EmployeeRepository
//{
//    private readonly TailorDbContext _context;

//    public EmployeeRepository(TailorDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<Employee>> GetAllAsync()
//    {
//        return await _context.Employees
//                             .Where(e => !e.IsDeleted)  // ✅ Exclude soft deleted employees
//                             .ToListAsync();
//    }

//    //public async Task<Employee> GetByIdAsync(int employeeId)
//    //{
//    //    return await _context.Employees
//    //                         .Where(e => e.EmployeeID == employeeId && !e.IsDeleted) // ✅ Exclude deleted employees
//    //                         .FirstOrDefaultAsync();
//    //}
//    public async Task<Employee?> GetByIdAsync(int employeeId)
//    {
//        return await _context.Employees
//                             .Where(e => e.EmployeeID == employeeId && !e.IsDeleted) // ✅ Exclude deleted employees
//                             .FirstOrDefaultAsync();
//    }

//    public async Task<Employee> AddAsync(Employee employee)
//    {
//        _context.Employees.Add(employee);
//        await _context.SaveChangesAsync();
//        return employee;
//    }

//    public async Task UpdateAsync(Employee employee)
//    {
//        _context.Employees.Update(employee);
//        await _context.SaveChangesAsync();
//    }
//}
