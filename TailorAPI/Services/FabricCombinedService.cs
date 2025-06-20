using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Services.Interface;
using TailorAPI.Models; // Added this line
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; // Add this 
using System.Linq;
using System.Threading.Tasks;
using TailorAPI.DTOs.Request;

namespace TailorAPI.Services
{
    public class FabricCombinedService : IFabricCombinedService
    {
        private readonly TailorDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FabricCombinedService(TailorDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // FabricType Methods
        public async Task<FabricTypeResponseDTO> AddFabricTypeAsync(FabricTypeRequestDTO request)
        {
            var shopId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("shopId")?.Value ?? "0");
            var branchId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("branchId")?.Value ?? "0");


            var fabricType = new FabricType
            {
                FabricName = request.FabricName,
                PricePerMeter = request.PricePerMeter,
                AvailableStock = request.AvailableStock,
                ShopId = shopId,
                BranchId = branchId,
                IsDeleted = false
            };

            _context.FabricTypes.Add(fabricType);
            await _context.SaveChangesAsync();

            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabricType.FabricTypeID,
                FabricName = fabricType.FabricName,
                PricePerMeter = fabricType.PricePerMeter,
                AvailableStock = fabricType.AvailableStock
            };
        }

        public async Task<FabricTypeResponseDTO> UpdateFabricTypePriceAsync(int id, decimal pricePerMeter)
        {
            var fabricType = await _context.FabricTypes.FindAsync(id);
            if (fabricType == null || fabricType.IsDeleted)
                throw new Exception("Fabric Type not found or deleted.");

            fabricType.PricePerMeter = pricePerMeter;
            await _context.SaveChangesAsync();

            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabricType.FabricTypeID,
                FabricName = fabricType.FabricName,
                PricePerMeter = fabricType.PricePerMeter,
                AvailableStock = fabricType.AvailableStock
            };
        }

        public async Task<IEnumerable<FabricTypeResponseDTO>> GetAllFabricTypesAsync() =>
            await _context.FabricTypes
                .Where(f => !f.IsDeleted)
                .AsNoTracking() // for database tracking issue 
                .Select(f => new FabricTypeResponseDTO
                {
                    FabricTypeID = f.FabricTypeID,
                    FabricName = f.FabricName,
                    PricePerMeter = f.PricePerMeter,
                    AvailableStock = f.AvailableStock
                }).ToListAsync();

        public async Task<FabricTypeResponseDTO> GetFabricTypeByIdAsync(int id)
        {
            var fabricType = await _context.FabricTypes.FindAsync(id);
            if (fabricType == null || fabricType.IsDeleted)
                throw new Exception("Fabric Type not found or deleted.");

            return new FabricTypeResponseDTO
            {
                FabricTypeID = fabricType.FabricTypeID,
                FabricName = fabricType.FabricName,
                PricePerMeter = fabricType.PricePerMeter,
                AvailableStock = fabricType.AvailableStock
            };
        }

        public async Task<bool> SoftDeleteFabricTypeAsync(int id)
        {
            var fabricType = await _context.FabricTypes.FindAsync(id);
            if (fabricType == null || fabricType.IsDeleted)
                return false;

            fabricType.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        } 


        // --------------FabriStockService-----------------


        // FabricStock Methods 
        public async Task<FabricStockResponseDTO> AddFabricStockAsync(FabricStockRequestDTO request)
        {
            var shopId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("shopId")?.Value ?? "0");
            var branchId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("branchId")?.Value ?? "0");

            var fabricType = await _context.FabricTypes.FindAsync(request.FabricTypeID);
            if (fabricType == null || fabricType.IsDeleted)
                throw new Exception("Fabric Type not found or deleted.");
            var fabricStock = new FabricStock
            {
                FabricTypeID = request.FabricTypeID,
                StockIn = request.StockIn,
                StockAddDate = request.StockAddDate,
                ShopId = shopId,
                BranchId = branchId,
                StockUse = 0 // Default for new entries
            };

            fabricType.AvailableStock += request.StockIn; // Stock Calculation Logic
            await _context.FabricStocks.AddAsync(fabricStock);
            await _context.SaveChangesAsync();




            return new FabricStockResponseDTO
            {
                StockID = fabricStock.StockID,
                FabricTypeID = fabricStock.FabricTypeID,
                StockIn = fabricStock.StockIn,
                StockOut = fabricStock.StockUse,
                StockAddDate = fabricStock.StockAddDate
            };
        }

        public async Task<IEnumerable<FabricStockResponseDTO>> GetAllFabricStocksAsync() =>
            await _context.FabricStocks
                .Select(fs => new FabricStockResponseDTO
                {
                    StockID = fs.StockID,
                    FabricTypeID = fs.FabricTypeID,
                    StockIn = fs.StockIn,
                    StockOut = fs.StockUse,
                    StockAddDate = fs.StockAddDate
                }).ToListAsync();
         
        public async Task<FabricStockResponseDTO> GetFabricStockByIdAsync(int id)
        {
            var fabricStock = await _context.FabricStocks.FindAsync(id);
            if (fabricStock == null)
                throw new Exception("Fabric Stock not found.");

            return new FabricStockResponseDTO
            {
                StockID = fabricStock.StockID,
                FabricTypeID = fabricStock.FabricTypeID,
                StockIn = fabricStock.StockIn,
                StockOut = fabricStock.StockUse,
                StockAddDate = fabricStock.StockAddDate
            };
        }
    }
}
