using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class FabricRepository
    {
        private readonly TailorDbContext _context;

        public FabricRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFabric(Fabric fabric)
        {
            _context.Fabrics.Add(fabric);
            await _context.SaveChangesAsync();
            return "Fabric added successfully.";
        }

        public async Task<List<Fabric>> GetAllFabrics()
        {
            return await _context.Fabrics.Where(f => !f.IsDeleted).ToListAsync();
        }

        public async Task<bool> DeleteFabric(int fabricId)
        {
            var fabric = await _context.Fabrics.FindAsync(fabricId);
            if (fabric == null) return false;

            fabric.IsDeleted = true;  // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Fabric> GetFabricById(int fabricId)
        {
            return await _context.Fabrics.FindAsync(fabricId);
        }


    }
}