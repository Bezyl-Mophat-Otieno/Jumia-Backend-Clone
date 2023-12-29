using Microsoft.EntityFrameworkCore;
using TransactionMS.Data;
using TransactionMS.Data.Dtos;
using TransactionMS.Models;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Services
{
    public class OrderService : IOrder
    {
        private readonly ApplicationDBContext _context;

        public OrderService(ApplicationDBContext context)
        {
            _context = context;
            
        }
        public async Task<string> CreateOrder(Order neworder)
        {
            try {

                await _context.Orders.AddAsync(neworder);
                await _context.SaveChangesAsync();
                return "";
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            try {

                var orders = await _context.Orders.Include(order=>order.Products).ToListAsync();
                return orders;
            
            
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            try {

                var order = await _context.Orders.Where(order=>order.Id == orderId).Include(order=>order.Products).FirstOrDefaultAsync();


                return order;

                }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Order>> GetOrderByUserId(Guid userId)
        {
            try {


                var orders = await _context.Orders.Where(order => order.UserId ==userId ).Include(order => order.Products).ToListAsync();


                return orders;



            }
            catch (Exception ex) {

                return null;
                    
                    
                    
                  }
        }
    }
}
