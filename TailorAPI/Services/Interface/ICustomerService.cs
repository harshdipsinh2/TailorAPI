using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.DTO.RequestDTO;

namespace TailorAPI.Services
{
    public interface ICustomerService
    {
        //Task<List<CustomerDTO>> GetAllCustomersAsync();

        Task<List<CustomerDTO>> GetCustomersForAdminAsync(int? shopId, int? branchId);
        Task<List<CustomerDTO>> GetCustomersForManagerAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> AddCustomerAsync(CustomerRequestDTO customerDto);
        Task<Customer> UpdateCustomerAsync(int customerId, CustomerRequestDTO customerDto);
        Task<List<CustomerDTO>> GetAllCustomersForSuperAdminAsync(int shopId , int? Branchid = null);

        Task<bool> SoftDeleteCustomerAsync(int customerId);
    }
}
