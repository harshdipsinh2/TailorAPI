using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;

namespace TailorAPI.Services.Interface
{
    public interface IFabricService
    {
        Task<string> AddFabric(FabricRequestDTO fabricDto);
        Task<List<FabricResponseDTO>> GetAllFabrics();
        Task<FabricResponseDTO> GetFabricById(int fabricId);  // New method
        Task<bool> DeleteFabric(int fabricId);
    }
}
