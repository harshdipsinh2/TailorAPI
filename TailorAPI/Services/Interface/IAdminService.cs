using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IAdminService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO?> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> AddCustomerAsync(CustomerRequestDTO customerDto);
        Task<Customer?> UpdateCustomerAsync(int customerId, CustomerRequestDTO customerDto);
        Task<bool> SoftDeleteCustomerAsync(int customerId);
        Task<MeasurementResponseDTO> AddMeasurementAsync(int customerId, MeasurementRequestDTO measurementDto);
        Task<MeasurementResponseDTO?> GetMeasurementByCustomerIDAsync(int customerId);
        Task<bool> SoftDeleteMeasurementAsync(int measurementId);
        Task<List<MeasurementResponseDTO>> GetAllMeasurementsAsync();
    }
}
