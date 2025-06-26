using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class BranchService : IBranchService
    {
        private readonly UserRepository _userRepository;
        private readonly TailorDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtService _JwtService;
        private readonly IShopService _shopService;
        private readonly BranchRepository _branchRepository;

        private readonly ShopRepository _shopRepository;

        public BranchService(
            UserRepository userRepository,
            JwtService jwtService,
            IShopService shopService,
            BranchRepository branchRepository,
            TailorDbContext context,
            ShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _JwtService = jwtService;
            _shopService = shopService;
            _branchRepository = branchRepository;
            _context = context;
            _shopRepository = shopRepository;
        }

        // ✅ Called from UserService when an Admin registers
        public async Task<Branch> CreateHeadBranchForShopAsync(Shop shop)
        {
            var headBranch = new Branch
            {
                BranchName = "HEAD-BRANCH",
                Location = shop.Location,
                ShopId = shop.ShopId,
                //PlanType = shop.PlanType 
            };

            await _branchRepository.CreateBranchAsync(headBranch);
            return headBranch;
        }

        // ✅ Used in /api/branch for admins creating additional branches
        public async Task<BranchResponseDTO?> CreateBranchAsync(BranchRequestDTO dto)
        {
            var shop = await _context.Shops
                .Include(s => s.Plan)
                .Include(s => s.Branches)
                .FirstOrDefaultAsync(s => s.ShopId == dto.ShopId);

            if (shop == null) throw new Exception("Shop not found");
            if (shop.Plan == null) throw new Exception("No active plan found for this shop");

            var currentBranchCount = shop.Branches.Count();

            if (currentBranchCount >= shop.Plan.MaxBranches)
                throw new Exception("Branch creation limit reached for your current plan.");

            var newBranch = new Branch
            {
                BranchName = dto.BranchName,
                Location = dto.Location,
                ShopId = dto.ShopId,
                PlanId = shop.PlanId
            };

            await _branchRepository.CreateBranchAsync(newBranch);

            return new BranchResponseDTO
            {
                BranchId = newBranch.BranchId,
                BranchName = newBranch.BranchName,
                Location = newBranch.Location,
                ShopId = newBranch.ShopId
            };
        }



    }
}
