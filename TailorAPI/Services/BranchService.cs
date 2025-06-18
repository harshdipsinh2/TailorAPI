using Microsoft.AspNetCore.Identity;
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
            ShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _JwtService = jwtService;
            _shopService = shopService;
            _branchRepository = branchRepository;
            _shopRepository = shopRepository;
        }

        // ✅ Called from UserService when an Admin registers
        public async Task<Branch> CreateHeadBranchForShopAsync(Shop shop)
        {
            var headBranch = new Branch
            {
                BranchName = "HEAD-BRANCH",
                Location = shop.Location,
                ShopId = shop.ShopId
            };

            await _branchRepository.CreateBranchAsync(headBranch);
            return headBranch;
        }

        // ✅ Used in /api/branch for admins creating additional branches
        public async Task<BranchResponseDTO?> CreateBranchAsync(BranchRequestDTO dto)
        {
            // Validate the shop exists
            var shop = await _shopRepository.GetShopByIdAsync(dto.ShopId);
            if (shop == null) return null;

            var newBranch = new Branch
            {
                BranchName = dto.BranchName,
                Location = dto.Location,
                ShopId = dto.ShopId
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
