using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class AdminService :  IAdminService
    {
        private readonly TailorDbContext _context;

        public AdminService(TailorDbContext context)
        {
            _context = context;
        }

        // GET ALL CUSTOMERS (Admin Only)
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

        // GET CUSTOMER BY ID (Admin Only)
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

        // ADD CUSTOMER (Admin Only)
        public async Task<CustomerDTO> AddCustomerAsync(CustomerRequestDTO customerDto)
        {
            var customer = new Customer
            {
                FullName = customerDto.FullName,
                PhoneNumber = customerDto.PhoneNumber,
                Email = customerDto.Email,
                Address = customerDto.Address,
                Gender = customerDto.Gender
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

        // UPDATE CUSTOMER (Admin Only)
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

        // DELETE CUSTOMER (Admin Only)
        public async Task<bool> SoftDeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Measurement)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null) return false;

            customer.IsDeleted = true;

            if (customer.Measurement != null)
            {
                customer.Measurement.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<MeasurementResponseDTO> AddMeasurementAsync(int customerId, MeasurementRequestDTO measurementDto)
        {
            var existingCustomer = await _context.Customers.FindAsync(customerId);
            if (existingCustomer == null)
                throw new ArgumentException("Customer does not exist.");

            var measurement = new Measurement
            {
                CustomerId = customerId,
                Chest = measurementDto.Chest,
                Waist = measurementDto.Waist,
                Hip = measurementDto.Hip,
                Shoulder = measurementDto.Shoulder,
                SleeveLength = measurementDto.SleeveLength,
                TrouserLength = measurementDto.TrouserLength,
                Inseam = measurementDto.Inseam,
                Thigh = measurementDto.Thigh,
                Neck = measurementDto.Neck,
                Sleeve = measurementDto.Sleeve,
                Arms = measurementDto.Arms,
                Bicep = measurementDto.Bicep,
                Forearm = measurementDto.Forearm,
                Wrist = measurementDto.Wrist,
                Ankle = measurementDto.Ankle,
                Calf = measurementDto.Calf
            };

            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            return new MeasurementResponseDTO
            {
                CustomerId = measurement.CustomerId,
                Chest = measurement.Chest,
                Waist = measurement.Waist,
                Hip = measurement.Hip,
                Shoulder = measurement.Shoulder,
                SleeveLength = measurement.SleeveLength,
                TrouserLength = measurement.TrouserLength,
                Inseam = measurement.Inseam,
                Thigh = measurement.Thigh,
                Neck = measurement.Neck,
                Sleeve = measurement.Sleeve,
                Arms = measurement.Arms,
                Bicep = measurement.Bicep,
                Forearm = measurement.Forearm,
                Wrist = measurement.Wrist,
                Ankle = measurement.Ankle,
                Calf = measurement.Calf
            };
        }

        public async Task<MeasurementResponseDTO> GetMeasurementByCustomerIDAsync(int customerId)
        {
            var measurement = await _context.Measurements.FirstOrDefaultAsync(m => m.CustomerId == customerId);
            if (measurement == null) return null;

            return new MeasurementResponseDTO
            {
                CustomerId = measurement.CustomerId,
                Chest = measurement.Chest,
                Waist = measurement.Waist,
                Hip = measurement.Hip,
                Shoulder = measurement.Shoulder,
                SleeveLength = measurement.SleeveLength,
                TrouserLength = measurement.TrouserLength,
                Inseam = measurement.Inseam,
                Thigh = measurement.Thigh,
                Neck = measurement.Neck,
                Sleeve = measurement.Sleeve,
                Arms = measurement.Arms,
                Bicep = measurement.Bicep,
                Forearm = measurement.Forearm,
                Wrist = measurement.Wrist,
                Ankle = measurement.Ankle,
                Calf = measurement.Calf
            };
        }

        public async Task<bool> SoftDeleteMeasurementAsync(int measurementId)
        {
            var measurement = await _context.Measurements.FirstOrDefaultAsync(m => m.MeasurementID == measurementId);
            if (measurement == null) return false;

            measurement.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MeasurementResponseDTO>> GetAllMeasurementsAsync()
        {
            var measurements = await _context.Measurements.Include(m => m.Customer)
                .Where(m => !m.IsDeleted)
                .Select(m => new MeasurementResponseDTO
                {
                    CustomerId = m.CustomerId,
                    Chest = m.Chest,
                    Waist = m.Waist,
                    Hip = m.Hip,
                    Shoulder = m.Shoulder,
                    SleeveLength = m.SleeveLength,
                    TrouserLength = m.TrouserLength,
                    Inseam = m.Inseam,
                    Thigh = m.Thigh,
                    Neck = m.Neck,
                    Sleeve = m.Sleeve,
                    Arms = m.Arms,
                    Bicep = m.Bicep,
                    Forearm = m.Forearm,
                    Wrist = m.Wrist,
                    Ankle = m.Ankle,
                    Calf = m.Calf
                })
                .ToListAsync();

            return measurements;
        }
    }
}
