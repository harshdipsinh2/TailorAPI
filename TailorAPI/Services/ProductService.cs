using System;
using TailorAPI.Models;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly TailorDbContext _context;

        public ProductService(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddProduct(int productID, string productName, decimal price)
        {
            if (_context.Products.Any(p => p.ProductID == productID))
            {
                return "Product ID already exists.";
            }

            var product = new Product
            {
                ProductID = productID,
                ProductName = productName,
                Price = price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return "Product added successfully.";
        }

        public async Task<List<Product>> GetProducts()
        {
            return await Task.FromResult(_context.Products.ToList());
        }
    }

}
