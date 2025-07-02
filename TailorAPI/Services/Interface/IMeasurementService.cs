using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;

namespace TailorAPI.Services.Interface
{
    public interface IMeasurementService
    {
        Task<MeasurementResponseDTO> AddMeasurementAsync(int customerId, MeasurementRequestDTO measurementDto);
        Task<MeasurementResponseDTO> GetMeasurementByCustomerIDAsync(int customerId);
        Task<bool> SoftDeleteMeasurementAsync(int measurementId);
        Task<List<MeasurementResponseDTO>> GetAllMeasurementsAsync();
        Task<List<MeasurementResponseDTO>> GetallMeasurementForSuperAdminAsync();
    }
}
