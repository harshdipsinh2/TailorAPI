using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class FabricTypeCombinedRepository
    {
        private readonly TailorDbContext _context;

        public FabricTypeCombinedRepository(TailorDbContext context )
        {
            _context = context;
        }

        // FabricType Methods
        public async Task<FabricTypeResponseDTO> AddFabricType(FabricTypeRequestDTO request)
        {
            var fabric = new FabricType
            {
                FabricName = request.FabricName,
                PricePerMeter = request.PricePerMeter,
                AvailableStock = request.AvailableStock,
                IsDeleted = false
            };
            _context.FabricTypes.Add(fabric);
            await _context.SaveChangesAsync();
            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabric.FabricTypeID,
                FabricName = fabric.FabricName,
                PricePerMeter = fabric.PricePerMeter,
                AvailableStock = fabric.AvailableStock
            };
        }

        public async Task<FabricTypeResponseDTO> UpdateFabricType(int id, decimal price)
        {
            var fabric = await _context.FabricTypes.FindAsync(id);
            if (fabric == null || fabric.IsDeleted) return null;

            fabric.PricePerMeter = price;
            await _context.SaveChangesAsync();
            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabric.FabricTypeID,
                FabricName = fabric.FabricName,
                PricePerMeter = fabric.PricePerMeter,
                AvailableStock = fabric.AvailableStock
            };
        }

        public async Task<FabricTypeResponseDTO> SoftDeleteFabricType(int id)
        {
            var fabric = await _context.FabricTypes.FindAsync(id);
            if (fabric == null || fabric.IsDeleted) return null;

            fabric.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabric.FabricTypeID,
                FabricName = fabric.FabricName,
                PricePerMeter = fabric.PricePerMeter,
                AvailableStock = fabric.AvailableStock
            };
        }

        // FabricStock Methods
        public async Task<FabricStockResponseDTO> AddFabricStock(FabricStockRequestDTO request)
        {
            var fabric = await _context.FabricTypes.FindAsync(request.FabricTypeID);
            if (fabric == null || fabric.IsDeleted) return null;

            var stock = new FabricStock
            {
                FabricTypeID = request.FabricTypeID,
                StockIn = request.StockIn,
                StockAddDate = request.StockAddDate
            };
            _context.FabricStocks.Add(stock);

            // Update Available Stock Calculation
            fabric.AvailableStock = fabric.AvailableStock + request.StockIn - fabric.Orders.Sum(o => o.FabricLength);
            await _context.SaveChangesAsync();

            return new FabricStockResponseDTO
            {
                StockID = stock.StockID,
                FabricTypeID = stock.FabricTypeID,
                StockIn = stock.StockIn,
                StockOut = fabric.Orders.Sum(o => o.FabricLength),
                StockAddDate = stock.StockAddDate
            };
        }

        //public async Task<List<FabricTypeResponseDTO>> GetAllFabricTypes()
        //{
        //    return await _context.FabricTypes
        //        .Include(c => c.Shop)
        //        .Include(c => c.Branch)
        //        .Where(f => !f.IsDeleted)

        //        .Select(f => new FabricTypeResponseDTO
        //        {
        //            FabricTypeID = f.FabricTypeID,
        //            FabricName = f.FabricName,
        //            PricePerMeter = f.PricePerMeter,
        //            AvailableStock = f.AvailableStock,

        //        }).ToListAsync();
        //}

        public async Task<List<FabricType>> GetAllFabricTypes()
        {
            return await _context.FabricTypes
                .Include(c => c.Shop)
                .Include(c => c.Branch)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }


        public async Task<FabricTypeResponseDTO> GetFabricTypeById(int id)
        {
            var fabric = await _context.FabricTypes.FindAsync(id);
            if (fabric == null || fabric.IsDeleted) return null;

            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabric.FabricTypeID,
                FabricName = fabric.FabricName,
                PricePerMeter = fabric.PricePerMeter,
                AvailableStock = fabric.AvailableStock
            };
        }

        public async Task<List<FabricStockResponseDTO>> GetAllFabricStocks()
        {
            return await _context.FabricStocks
                .Include(f => f.FabricType)
                .Select(s => new FabricStockResponseDTO
                {
                    StockID = s.StockID,
                    FabricTypeID = s.FabricTypeID,
                    StockIn = s.StockIn,
                    StockOut = s.FabricType.Orders.Sum(o => o.FabricLength),
                    StockAddDate = s.StockAddDate
                }).ToListAsync();
        }

        public async Task<FabricStockResponseDTO> GetFabricStockById(int id)
        {
            var stock = await _context.FabricStocks
                .Include(f => f.FabricType)
                .FirstOrDefaultAsync(s => s.StockID == id);

            if (stock == null) return null;

            return new FabricStockResponseDTO
            {
                StockID = stock.StockID,
                FabricTypeID = stock.FabricTypeID,
                StockIn = stock.StockIn,
                StockOut = stock.FabricType.Orders.Sum(o => o.FabricLength),
                StockAddDate = stock.StockAddDate
            };
        }
    }
}
