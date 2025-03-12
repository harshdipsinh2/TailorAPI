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

        public async Task<string> AddProduct(int productID, string productName, decimal makingPrice)
        {
            var product = new Product
            {
                ProductID = productID,
                ProductName = productName,
                MakingPrice = makingPrice
            };
            return await _productRepository.AddProduct(product);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }
        public async Task<Product> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }

    }
}
