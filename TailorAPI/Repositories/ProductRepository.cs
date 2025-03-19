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

        // Correct return type for AddProduct
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;  // Return the Product object instead of string
        }
        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == productId && !p.IsDeleted);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
        // UpdateProduct Method
        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // GetAllProducts Method
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

    }
}
