using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;

namespace TailorAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TailorDbContext _context;

        public OrderService(OrderRepository orderRepository, TailorDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricId, OrderRequestDto request)
        {
            var product = await _context.Products.FindAsync(productId);
            var fabric = await _context.Fabrics.FindAsync(fabricId);

            if (product == null || fabric == null) throw new Exception("Invalid Product or Fabric");

            var totalPrice = ((decimal)request.FabricLength * fabric.PricePerMeter)
                + ((decimal)product.MakingPrice * request.Quantity);

            var order = new Order
            {
                CustomerId = customerId,
                ProductID = productId,
                FabricID = fabricId,
                FabricLength = (decimal)request.FabricLength,
                Quantity = request.Quantity,
                TotalPrice = totalPrice,
                CompletionDate = request.CompletionDate
            };

            await _orderRepository.AddOrderAsync(order);

            return new OrderResponseDto
            {
                CustomerName = (await _context.Customers.FindAsync(customerId))?.FullName,
                ProductName = product.ProductName,
                FabricName = fabric.FabricName,
                FabricLength = (decimal)request.FabricLength,
                Quantity = request.Quantity,
                TotalPrice = totalPrice,
                CompletionDate = request.CompletionDate.ToString("yyyy-MM-dd")
            };
        }

        public async Task<bool> UpdateOrderAsync(int id, int productId, int fabricId, OrderRequestDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            var product = await _context.Products.FindAsync(productId);
            var fabric = await _context.Fabrics.FindAsync(fabricId);

            if (product == null || fabric == null) throw new Exception("Invalid Product or Fabric");

            order.FabricLength = (decimal)request.FabricLength;
            order.Quantity = request.Quantity;
            order.TotalPrice = ((decimal)request.FabricLength * fabric.PricePerMeter)
                   + ((decimal)product.MakingPrice * (decimal)request.Quantity);

            order.CompletionDate = request.CompletionDate;

            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            order.IsDeleted = true;
            await _orderRepository.UpdateOrderAsync(order);

            return true;
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

            var product = await _context.Products.FindAsync(order.ProductID);
            var fabric = await _context.Fabrics.FindAsync(order.FabricID);
            var customer = await _context.Customers.FindAsync(order.CustomerId);

            return new OrderResponseDto
            {
                CustomerName = customer?.FullName,
                ProductName = product?.ProductName,
                FabricName = fabric?.FabricName,
                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                CompletionDate = order.CompletionDate.ToString("yyyy-MM-dd")
            };
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            var orderDtos = new List<OrderResponseDto>();
            foreach (var order in orders)
            {
                var product = await _context.Products.FindAsync(order.ProductID);
                var fabric = await _context.Fabrics.FindAsync(order.FabricID);
                var customer = await _context.Customers.FindAsync(order.CustomerId);

                orderDtos.Add(new OrderResponseDto
                {
                    CustomerName = customer?.FullName,
                    ProductName = product?.ProductName,
                    FabricName = fabric?.FabricName,
                    FabricLength = order.FabricLength,
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                    CompletionDate = order.CompletionDate.ToString("yyyy-MM-dd")
                });
            }

            return orderDtos;
        }
    }
}