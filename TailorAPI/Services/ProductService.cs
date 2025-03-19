using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;

namespace TailorAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDTO> AddProduct(ProductRequestDTO productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                MakingPrice = productDto.MakingPrice
            };
            var result = await _productRepository.AddProduct(product);
            return new ProductResponseDTO
            {
                ProductName = result.ProductName,
                MakingPrice = result.MakingPrice
            };
        }

        public async Task<ProductResponseDTO> UpdateProduct(int id, ProductRequestDTO productDto)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            product.ProductName = productDto.ProductName;
            product.MakingPrice = productDto.MakingPrice;

            var updatedProduct = await _productRepository.UpdateProduct(product);
            return new ProductResponseDTO
            {
                ProductID = updatedProduct.ProductID,
                ProductName = updatedProduct.ProductName,
                MakingPrice = updatedProduct.MakingPrice
            };
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }

        public async Task<ProductResponseDTO> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            return new ProductResponseDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                MakingPrice = product.MakingPrice
            };
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products.Select(product => new ProductResponseDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                MakingPrice = product.MakingPrice
            });
        }
    }
}