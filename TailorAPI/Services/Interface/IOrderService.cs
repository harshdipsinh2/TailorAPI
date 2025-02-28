using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IOrderService
    {
        Task<string> AddOrder(int customerID, int productID, int employeeID, int quantity);
        Task<List<Order>> GetOrders();
    }

}
