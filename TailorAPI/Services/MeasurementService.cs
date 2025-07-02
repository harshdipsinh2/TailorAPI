using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly TailorDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MeasurementService(TailorDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }


        private (float upper, float lower) CalculateBodyMeasurements(MeasurementRequestDTO dto)
        {
            float upper = dto.Chest + dto.Waist + dto.Shoulder + dto.SleeveLength + dto.Neck + dto.Sleeve + dto.Bicep + dto.Forearm;
            float lower = dto.Hip + dto.TrouserLength + dto.Inseam + dto.Thigh + dto.Calf + dto.Ankle;

            // Convert cm to meters if needed
            upper = (float)Math.Round(upper / 100f, 2);
            lower = (float)Math.Round(lower / 100f, 2);

            return (upper, lower);
        }

        public async Task<MeasurementResponseDTO> AddMeasurementAsync(int customerId, MeasurementRequestDTO measurementDto)
        {
            var shopId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("shopId")?.Value ?? "0");
            var branchId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("branchId")?.Value ?? "0");


            var existingCustomer = await _context.Customers.FindAsync(customerId);
            if (existingCustomer == null)
                throw new ArgumentException("Customer does not exist.");

            var (upper, lower) = CalculateBodyMeasurements(measurementDto);

            var measurement = new Measurement
            {
                CustomerId = customerId,
                ShopId = shopId,
                BranchId = branchId,
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
                Calf = measurementDto.Calf,
                UpperBodyMeasurement = upper,
                LowerBodyMeasurement = lower
            };




            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            return new MeasurementResponseDTO
            {
                CustomerId = measurement.CustomerId, // Include CustomerID in response

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
                Calf = measurement.Calf,
                UpperBodyMeasurement = upper,
                LowerBodyMeasurement = lower

            };
        }

        public async Task<MeasurementResponseDTO> GetMeasurementByCustomerIDAsync(int customerId)
        {
            var measurement = await _context.Measurements
                .FirstOrDefaultAsync(m => m.CustomerId == customerId);

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
                Calf = measurement.Calf,
                UpperBodyMeasurement = measurement.UpperBodyMeasurement,
                LowerBodyMeasurement = measurement.LowerBodyMeasurement

            };
        }

        public async Task<bool> SoftDeleteMeasurementAsync(int measurementId)
        {
            var measurement = await _context.Measurements
                .FirstOrDefaultAsync(m => m.MeasurementID == measurementId);

            if (measurement == null) return false;

            measurement.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MeasurementResponseDTO>> GetAllMeasurementsAsync()
        {

            var user = _httpContextAccessor.HttpContext.User;
            var role = user.FindFirst("role")?.Value;
            var shopId = int.Parse(user.FindFirst("shopId")?.Value ?? "0");
            var branchId = int.Parse(user.FindFirst("branchId")?.Value ?? "0");


            if (role == "Tailor")
            {
                // Option 1: Return empty list (silent failure)
                //return new List<CustomerDTO>();

                // Option 2: Throw exception (visible failure)
                throw new UnauthorizedAccessException("Tailor is not authorized to access customer data.");
            }
            return await _context.Measurements
                .Where(m => !m.IsDeleted && m.ShopId == shopId && m.BranchId == branchId)
                .Include(m => m.Shop)
                .Include(m => m.Branch)
                .AsNoTracking()
                .Select(m => new MeasurementResponseDTO
                {


                    MeasurementID = m.MeasurementID,
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
                    Calf = m.Calf,
                    UpperBodyMeasurement = m.UpperBodyMeasurement,
                    LowerBodyMeasurement = m.LowerBodyMeasurement

                })
                .ToListAsync();



        }
        public async Task<List<MeasurementResponseDTO>> GetallMeasurementForSuperAdminAsync()
        {
            return await _context.Measurements
                .Where(m => !m.IsDeleted)
                .Include(m => m.Shop)
                .Include(m => m.Branch)
                .AsNoTracking()
                .Select(m => new MeasurementResponseDTO
                {
                    MeasurementID = m.MeasurementID,
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
                    Calf = m.Calf,
                    UpperBodyMeasurement = m.UpperBodyMeasurement,
                    LowerBodyMeasurement = m.LowerBodyMeasurement
                })
                .ToListAsync();
        }
    }
}
