using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;

public class CustomerService : ICustomerService
{
    private readonly TailorDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CustomerService(TailorDbContext context , IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<CustomerDTO>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Where(c => !c.IsDeleted)
            .AsNoTracking()
            .Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                Gender = c.Gender
            })
            .ToListAsync();
    }

    public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return null;

        return new CustomerDTO
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address,
            Gender = customer.Gender
        };
    }

    public async Task<CustomerDTO> AddCustomerAsync(CustomerRequestDTO customerDto)
    {

        var shopId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("shopId")?.Value ?? "0");
        var branchId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("branchId")?.Value ?? "0");

        var customer = new Customer
        {
            FullName = customerDto.FullName,
            PhoneNumber = customerDto.PhoneNumber,
            Email = customerDto.Email,
            Address = customerDto.Address,
            Gender = customerDto.Gender,
            ShopId = shopId,
            BranchId = branchId
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerDTO
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address,
            Gender = customer.Gender
        };
    }

    public async Task<Customer> UpdateCustomerAsync(int customerId, CustomerRequestDTO customerDto)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return null;

        customer.FullName = customerDto.FullName;
        customer.PhoneNumber = customerDto.PhoneNumber;
        customer.Email = customerDto.Email;
        customer.Address = customerDto.Address;
        customer.Gender = customerDto.Gender;

        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> SoftDeleteCustomerAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Measurement) // ✅ Ensure Measurement is included
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (customer == null) return false;

        customer.IsDeleted = true;

        if (customer.Measurement != null) // ✅ Null check for Measurement
        {
            customer.Measurement.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }

}
