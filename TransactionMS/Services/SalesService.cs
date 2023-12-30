using AutoMapper;
using TransactionMS.Data;
using TransactionMS.Data.Dtos;
using TransactionMS.Models;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Services
{
    public class SalesService : ISales
    {

        private readonly IOrder _orderservice;

        private readonly IProduct _productservice;

        private readonly ApplicationDBContext _dbcontext;

        private readonly IMapper _mapper;

        public SalesService(IOrder orderservice , IProduct productservice , ApplicationDBContext dbcontext , IMapper mapper)
        {

            _orderservice = orderservice;

            _productservice = productservice;
            _dbcontext = dbcontext;
            _mapper = mapper;
            
        }



        public async Task<string> CreateSale(Guid OrderId)
        {
            try { 

                var order = await _orderservice.GetOrderById(OrderId);

                if (order == null)
                {
                    return "Order Not Found";
                }

                var products = order.Products.ToList();

                if (products.Count > 0)
                {
                    var productstobesold = await _productservice.ProductsToBeSold(products);

                    int? totalcost = 0;


                    productstobesold.ForEach(p =>
                    {
                        totalcost += p.Quantity * (int)p.Price;

                    });




                    var mappedproductstobesold = _mapper.Map<List<ProductSales>>(productstobesold);
                    

                    var sales1 = new Sales(){ 
                        OrderId = OrderId,
                        Products = mappedproductstobesold,
                        TotalCost = (double)totalcost
                    };

                    await _dbcontext.Sales.AddAsync(sales1);
                    await _dbcontext.SaveChangesAsync();

                    return "";

                }

                return "";

            } catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
