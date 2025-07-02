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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShopRepository _shopRepository;

        public BranchService(
            UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            JwtService jwtService,
            IShopService shopService,
            BranchRepository branchRepository,
            TailorDbContext context,
            ShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
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


        private int? GetCurrentShopId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("shopId");
            return int.TryParse(claim?.Value, out int shopId) ? shopId : null;
        }
        private string? GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst("roles")?.Value?.Trim().ToLower();
        }


        public async Task<BranchResponseDTO?> CreateBranchAsync(BranchRequestDTO dto)
        {
            var shopId = GetCurrentShopId();
            if (shopId == null) throw new Exception("Unauthorized: No ShopId found in token.");

            var shop = await _context.Shops
                .Include(s => s.Plan)
                .Include(s => s.Branches)
                .FirstOrDefaultAsync(s => s.ShopId == shopId);

            if (shop == null) throw new Exception("Shop not found");
            if (shop.Plan == null) throw new Exception("No active plan found for this shop");

            if (shop.Branches.Count >= shop.Plan.MaxBranches)
                throw new Exception("Branch creation limit reached for your current plan.");

            var newBranch = new Branch
            {
                BranchName = dto.BranchName,
                Location = dto.Location,
                PlanId = shop.PlanId,
                ShopId = shop.ShopId // ✅ Now set only here
            };

            await _branchRepository.CreateBranchAsync(newBranch);

            return new BranchResponseDTO
            {
                BranchId = newBranch.BranchId,
                BranchName = newBranch.BranchName,
                Location = newBranch.Location,
                ShopId = newBranch.ShopId,
                ShopName = shop.ShopName
            };
        }


        public async Task<List<BranchResponseDTO>> GetAllBranchesAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var role = user.FindFirst("role")?.Value;
            var shopId = int.Parse(user.FindFirst("shopId")?.Value ?? "0");

            if (role == "Tailor")
            {
                throw new UnauthorizedAccessException("Tailor is not authorized to access branch data.");
            }

            return await _context.Branches
                .Where(c => c.ShopId == shopId) // ✅ Show all branches of the shop
                .Include(c => c.Shop)
                .AsNoTracking()
                .Select(c => new BranchResponseDTO
                {
                    BranchId = c.BranchId,
                    BranchName = c.BranchName,
                    Location = c.Location,
                    ShopId = c.ShopId,
                    ShopName = c.Shop.ShopName
                }).ToListAsync();
        }





        public async Task<List<BranchResponseDTO>> GetAllBranchForSuperAdmin()
        {
            return await _context.Branches
                .Include(b => b.Shop)
                .AsNoTracking()
                .Select(b => new BranchResponseDTO
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    Location = b.Location,
                    ShopId = b.ShopId,
                    ShopName = b.Shop.ShopName

                })
                .ToListAsync();



        }

    }
}