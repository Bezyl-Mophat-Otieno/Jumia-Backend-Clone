using TransactionMS.Data.Dtos;
using TransactionMS.Models;

namespace TransactionMS.Services.Iservice
{
    public interface IOrder
    {
        Task<string> CreateOrder(Order neworder);
        Task<List<Order>> GetOrderByUserId(Guid userId);
        Task<List<Order>> GetAllOrders();

        Task<Order> GetOrderById(Guid orderId);


    }
}
