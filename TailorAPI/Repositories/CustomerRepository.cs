namespace TailorAPI.Repositories
{
    public class CustomerRepository
    {
        private readonly TailorDbContext _context;

        public CustomerRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
    }
}

