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
    }

}
