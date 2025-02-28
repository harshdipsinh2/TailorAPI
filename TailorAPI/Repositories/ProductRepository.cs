using System;
using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class ProductRepository
    {
        private readonly TailorDbContext _context;

        public ProductRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddProduct(int productID, string productName, decimal price)
        {
            if (await _context.Products.AnyAsync(p => p.ProductID == productID))
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
            return await _context.Products.ToListAsync();
        }
    }


}
