using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IProductService
    {
        Task<string> AddProduct(int productID, string productName, decimal price);
        Task<List<Product>> GetProducts();
    }

}
