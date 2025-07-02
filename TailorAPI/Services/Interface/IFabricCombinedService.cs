using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;

namespace TailorAPI.Services.Interface
{
    public interface IFabricCombinedService
    {
        // FabricType Methods
        Task<FabricTypeResponseDTO> AddFabricTypeAsync(FabricTypeRequestDTO request);
        Task<FabricTypeResponseDTO> UpdateFabricTypePriceAsync(int id, decimal pricePerMeter);
        Task<FabricTypeResponseDTO> GetFabricTypeByIdAsync(int id);
        Task<bool> SoftDeleteFabricTypeAsync(int id);
        Task<List<FabricTypeResponseDTO>> GetAllFabricTypeForSuperAdmin(int shopId, int? branchId );
        Task<List<FabricTypeResponseDTO>> GetFabriTypeForAdminAsync(int? shopId, int? branchId);
        Task<List<FabricTypeResponseDTO>> GetAllFabricTypeForManager();


        // FabricStock Methods
        Task<FabricStockResponseDTO> AddFabricStockAsync(FabricStockRequestDTO request);
        Task<IEnumerable<FabricStockResponseDTO>> GetAllFabricStocksAsync();
        Task<FabricStockResponseDTO> GetFabricStockByIdAsync(int id);
    }
}
