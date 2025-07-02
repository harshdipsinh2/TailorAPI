using System.Threading.Tasks;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;

namespace TailorAPI.Services
{
    public interface IProductService
    {
        Task<ProductResponseDTO> AddProduct(ProductRequestDTO productDto);
        Task<ProductResponseDTO> UpdateProduct(int id, ProductRequestDTO productDto);
        Task<bool> DeleteProduct(int id);
        Task<ProductResponseDTO> GetProductById(int id);
        Task<IEnumerable<ProductResponseDTO>> GetProductForAdminAsync(int? shopId, int? branchId);
        Task<List<ProductResponseDTO>> GetProductForManagerAsync();
        Task<List<ProductResponseDTO>> GetAllProductsForSuperAdmin(int shopId,int? branchId);   
    }
}