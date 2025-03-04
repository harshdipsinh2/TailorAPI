using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> AddProduct(int productID, string productName, decimal price)
        {
            var product = new Product
            {
                ProductID = productID,
                ProductName = productName,
                Price = price
            };
            return await _productRepository.AddProduct(product);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productRepository.GetProducts(); // Use repository method
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            return await _productRepository.DeleteProduct(productId); // Directly use repository method
        }
    }
}
