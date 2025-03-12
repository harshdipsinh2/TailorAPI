using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<string> AddProduct(Product product)
        {
            if (await _context.Products.AnyAsync(p => p.ProductID == product.ProductID))
            {
                return "Product ID already exists.";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return "Product added successfully.";
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Where(p => !p.IsDeleted).ToListAsync(); // Soft delete check
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }


    }
}
