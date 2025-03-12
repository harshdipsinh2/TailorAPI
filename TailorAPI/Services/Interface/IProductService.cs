using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IProductService
    {
        Task<string> AddProduct(int productID, string productName, decimal makingPrice);
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int productId);  // New method
        Task<bool> DeleteProduct(int productId);
    }
}
