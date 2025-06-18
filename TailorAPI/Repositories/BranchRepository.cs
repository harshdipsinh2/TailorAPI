using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class BranchRepository
    {
        private readonly TailorDbContext _context;
        public BranchRepository(TailorDbContext context)
        {
            _context = context;
        }



        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

    }
}
