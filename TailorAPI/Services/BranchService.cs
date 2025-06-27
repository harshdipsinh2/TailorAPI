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


        public async Task<List<BranchResponseDTO>> GetAllBranchesAsync(int? shopId = null)
        {
            var role = GetCurrentUserRole(); // returns lowercase
            var tokenShopId = GetCurrentShopId();

            IQueryable<Branch> query = _context.Branches.Include(b => b.Shop);

            if (role == "admin")
            {
                if (tokenShopId == null)
                    throw new Exception("Unauthorized: ShopId not found in token.");
                query = query.Where(b => b.ShopId == tokenShopId);
            }
            else if (role == "superadmin")
            {
                if (shopId.HasValue)
                    query = query.Where(b => b.ShopId == shopId.Value);
                // else return all branches
            }
            else
            {
                throw new Exception("Unauthorized: Only Admin or SuperAdmin can access branches.");
            }

            var branches = await query.ToListAsync();

            return branches.Select(b => new BranchResponseDTO
            {
                BranchId = b.BranchId,
                BranchName = b.BranchName,
                Location = b.Location,
                ShopId = b.ShopId,
                ShopName = b.Shop?.ShopName
            }).ToList();
        }

    }
}