using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TailorAPI.DTO;
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

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();

            return orders.Select(o => new OrderResponseDto
            {
                CustomerName = o.Customer?.FullName ?? "Unknown",
                ProductName = o.Product?.ProductName ?? "Unknown",
                Quantity = o.Quantity,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus,
                OrderDate = o.OrderDate.ToString("dd-MM-yyyy"), // Format as string
                CompletionDate = o.CompletionDate?.ToString("dd-MM-yyyy") // Format as string
            }).ToList();
        }

        public async Task<OrderResponseDto> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null) return null;

            return new OrderResponseDto
            {
                CustomerName = order.Customer?.FullName ?? "Unknown",
                ProductName = order.Product?.ProductName ?? "Unknown",
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                OrderDate = order.OrderDate.ToString("dd-MM-yyyy"), // Format as string
                CompletionDate = order.CompletionDate?.ToString("dd-MM-yyyy") // Format as string
            };
        }

        public async Task<OrderResponseDto> CreateOrder(OrderResponseDto request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.FullName == request.CustomerName);
            if (customer == null) return null;

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == request.ProductName);
            if (product == null) return null;

            var totalPrice = product.Price * request.Quantity;

            // Parse the OrderDate from the string
            if (!DateTime.TryParseExact(request.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var orderDate))
            {
                throw new ArgumentException("Invalid OrderDate format. Expected format: dd-MM-yyyy");
            }

            // Parse the CompletionDate from the string if provided
            DateTime? completionDate = null;
            if (!string.IsNullOrEmpty(request.CompletionDate))
            {
                if (!DateTime.TryParseExact(request.CompletionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCompletionDate))
                {
                    throw new ArgumentException("Invalid CompletionDate format. Expected format: dd-MM-yyyy");
                }
                completionDate = parsedCompletionDate;
            }

            var order = new Order
            {
                CustomerID = customer.CustomerID,
                ProductID = product.ProductID,
                Quantity = request.Quantity,
                TotalPrice = totalPrice,
                OrderStatus = request.OrderStatus ?? "Pending",
                PaymentStatus = request.PaymentStatus ?? "Pending",
                OrderDate = orderDate,
                CompletionDate = completionDate
            };

            await _orderRepository.AddOrder(order);

            return new OrderResponseDto
            {
                CustomerName = customer.FullName,
                ProductName = product.ProductName,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                OrderDate = order.OrderDate.ToString("dd-MM-yyyy"),
                CompletionDate = order.CompletionDate?.ToString("dd-MM-yyyy")
            };
        }

        public async Task<OrderUpdateDto> UpdateOrder(int orderId, int quantity, string orderStatus, string paymentStatus, string completionDate)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null) return null;

            order.Quantity = quantity;
            order.OrderStatus = orderStatus;
            order.PaymentStatus = paymentStatus;

            // Parse the CompletionDate from the string if provided
            if (!string.IsNullOrEmpty(completionDate))
            {
                if (!DateTime.TryParseExact(completionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCompletionDate))
                {
                    throw new ArgumentException("Invalid CompletionDate format. Expected format: dd-MM-yyyy");
                }
                order.CompletionDate = parsedCompletionDate;
            }
            else
            {
                order.CompletionDate = null;
            }

            await _orderRepository.UpdateOrder(order);

            return new OrderUpdateDto
            {
                Quantity = order.Quantity,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                CompletionDate = order.CompletionDate?.ToString("dd-MM-yyyy")
            };
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null) return false;

            await _orderRepository.DeleteOrder(order);
            return true;
        }
    }
}