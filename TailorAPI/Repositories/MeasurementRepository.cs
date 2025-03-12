using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class MeasurementRepository
    {
        private readonly TailorDbContext _context;

        public MeasurementRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<Measurement> AddMeasurementAsync(Measurement measurement)
        {
            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();
            return measurement;
        }

        public async Task<bool> SoftDeleteMeasurementAsync(int measurementId)
        {
            var measurement = await _context.Measurements.FindAsync(measurementId);
            if (measurement == null) return false;

            measurement.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Measurement>> GetAllMeasurementsAsync()
        {
            return await _context.Measurements
                .Include(m => m.Customer)
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }
    }
}
