using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class FabricService : IFabricService
    {
        private readonly FabricRepository _fabricRepository;

        public FabricService(FabricRepository fabricRepository)
        {
            _fabricRepository = fabricRepository;
        }

        public async Task<string> AddFabric(FabricRequestDTO fabricDto)
        {
            var fabric = new Fabric
            {
                FabricName = fabricDto.FabricName,
                PricePerMeter = fabricDto.PricePerMeter,
                StockQuantity = fabricDto.StockQuantity
            };
            return await _fabricRepository.AddFabric(fabric);
        }

        public async Task<List<FabricResponseDTO>> GetAllFabrics()
        {
            var fabrics = await _fabricRepository.GetAllFabrics();
            return fabrics.Select(f => new FabricResponseDTO
            {
                FabricName = f.FabricName,
                PricePerMeter = f.PricePerMeter,
                StockQuantity = f.StockQuantity
            }).ToList();
        }

        public async Task<bool> DeleteFabric(int fabricId)
        {
            return await _fabricRepository.DeleteFabric(fabricId);
        }
        public async Task<FabricResponseDTO> GetFabricById(int fabricId)
        {
            var fabric = await _fabricRepository.GetFabricById(fabricId);
            if (fabric == null) return null;

            return new FabricResponseDTO
            {
                FabricName = fabric.FabricName,
                PricePerMeter = fabric.PricePerMeter,
                StockQuantity = fabric.StockQuantity
            };
        }


    }
}