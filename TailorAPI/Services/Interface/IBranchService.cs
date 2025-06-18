using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IBranchService
    {
        Task<Branch> CreateHeadBranchForShopAsync(Shop shop);
        Task<BranchResponseDTO?> CreateBranchAsync(BranchRequestDTO dto);
    }
}
