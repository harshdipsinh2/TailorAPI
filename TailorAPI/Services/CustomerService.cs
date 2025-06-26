using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;
using TailorAPI.Services.Interface;

public class CustomerService : ICustomerService
{
    private readonly TailorDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessScopeService _accessScope;
    public CustomerService(TailorDbContext context , IHttpContextAccessor httpContextAccessor, IAccessScopeService accessScope)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _accessScope = accessScope;
    }

    public async Task<List<CustomerDTO>> GetAllCustomersAsync()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var role = user.FindFirst("role")?.Value;
        var shopId = int.Parse(user.FindFirst("shopId")?.Value ?? "0");
        var branchId = int.Parse(user.FindFirst("branchId")?.Value ?? "0");

        // Restrict access for Tailor role
        if (role == "Tailor")
        {
            // Option 1: Return empty list (silent failure)
            //return new List<CustomerDTO>();

            // Option 2: Throw exception (visible failure)
            throw new UnauthorizedAccessException("Tailor is not authorized to access customer data.");
        }
        return await _context.Customers
            .Where(c => !c.IsDeleted && c.ShopId == shopId && c.BranchId == branchId)
            .Include(c => c.Shop)
            .Include(c => c.Branch)
            .AsNoTracking()
            .Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                ShopId = c.ShopId,
                BranchId = c.BranchId,
                ShopName = c.Shop.ShopName,
                BranchName = c.Branch.BranchName,
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

        // Check if the shop exists
        var shop = await _context.Shops.FindAsync(shopId);
        if (shop == null)
            throw new Exception("Shop not found.");

        // Count existing (not deleted) customers for this shop
        var customerCount = await _context.Customers
            .Where(c => c.ShopId == shopId && !c.IsDeleted)
            .CountAsync();

        // If no plan, allow only 2 customers
        if (shop.PlanId == null && customerCount >= 2)
            throw new InvalidOperationException("Trial limit reached. Please purchase a plan to add more customers.");

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
