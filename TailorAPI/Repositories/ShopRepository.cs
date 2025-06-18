using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class ShopRepository
    {
        private readonly TailorDbContext _context;


        public ShopRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<Shop> CreateShopAsync(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        public async Task<Shop?> GetShopByIdAsync(int shopId)
        {
            return await _context.Shops.FindAsync(shopId);
        }
        public async Task<bool> UpdateShopAsync(Shop shop)
        {
            _context.Shops.Update(shop);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
