using Microsoft.EntityFrameworkCore;

public class MeasurementService
{
    private readonly TailorDbContext _context;

    public MeasurementService(TailorDbContext context)
    {
        _context = context;
    }

    public async Task<Measurement> AddMeasurementAsync(MeasurementDTO measurementDto)
    {
        // Check if the customer exists
        var existingCustomer = await _context.Customers.FindAsync(measurementDto.CustomerID);

        if (existingCustomer == null)
        {
            throw new ArgumentException("Customer does not exist.");
        }

        // Check if a measurement already exists for this customer
        var existingMeasurement = await _context.Measurements
            .FirstOrDefaultAsync(m => m.CustomerID == measurementDto.CustomerID);

        if (existingMeasurement != null)
        {
            throw new InvalidOperationException("Measurement for this customer already exists.");
        }

        var measurement = new Measurement
        {
            CustomerID = measurementDto.CustomerID, // FK
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
            Customer = existingCustomer // ✅ Ensures proper relationship
        };

        _context.Measurements.Add(measurement);
        await _context.SaveChangesAsync();

        return measurement;
    }

    public async Task<MeasurementDTO> GetMeasurementByCustomerIDAsync(int customerId)
    {
        var measurement = await _context.Measurements
            .FirstOrDefaultAsync(m => m.CustomerID == customerId);

        if (measurement == null) return null;

        return new MeasurementDTO
        {
            CustomerID = measurement.CustomerID,
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
            Arms = measurement.Arms
        };
    }

    public async Task<bool> SoftDeleteMeasurementAsync(int measurementId)
    {
        var measurement = await _context.Measurements
            .FirstOrDefaultAsync(m => m.MeasurementID == measurementId); // ✅ Correct filtering

        if (measurement == null) return false;

        // ✅ Soft delete Measurement
        measurement.IsDeleted = true;

        // ✅ Soft delete Employees assigned to this Measurement
        //var employees = await _context.Employees.Where(e => e.MeasurementID == measurementId).ToListAsync();
        //foreach (var emp in employees)
        //{
        //    emp.MeasurementID = null; // Remove assigned Measurement
        //}

        await _context.SaveChangesAsync();
        return true;
    }



}
