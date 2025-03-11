using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly TailorDbContext _context;

    public CustomerService(TailorDbContext context)
    {
        _context = context;
    }

    // ✅ Get all customers
    public async Task<List<CustomerDTO>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Where(c => !c.IsDeleted) // Exclude soft-deleted records
            .Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address
            })
            .ToListAsync();
    }

    public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return null;

        return new CustomerDTO
        {
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address
        };
    }


    // ✅ Add a new customer
    public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO customerDto)
    {
        var customer = new Customer
        {
            FullName = customerDto.FullName,
            PhoneNumber = customerDto.PhoneNumber,
            Email = customerDto.Email,
            Address = customerDto.Address
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerDTO
        {
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address
        };
    }




    // ✅ Update existing customer
    public async Task<Customer> UpdateCustomerAsync(int customerId, CustomerDTO customerDto)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return null;

        customer.FullName = customerDto.FullName;
        customer.PhoneNumber = customerDto.PhoneNumber;
        customer.Email = customerDto.Email;
        customer.Address = customerDto.Address;

        await _context.SaveChangesAsync();
        return customer;
    }

    // ✅Delete customer
    public async Task<bool> SoftDeleteCustomerAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return false;

        //  Soft delete Customer
        customer.IsDeleted = true;

        // (It will be deleted auto)
        var measurement = await _context.Measurements.FirstOrDefaultAsync(m => m.CustomerID == customerId);

      

        await _context.SaveChangesAsync();
        return true;
    }


}
