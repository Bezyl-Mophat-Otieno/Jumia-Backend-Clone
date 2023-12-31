using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using TransactionMS.Data;
using TransactionMS.Data.Dto;
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

        private readonly StripeRequestDTO _requestdto;


        private readonly IMapper _mapper;


        public SalesService(IOrder orderservice , IProduct productservice , ApplicationDBContext dbcontext , IMapper mapper )
        {

            _orderservice = orderservice;

            _productservice = productservice;
            _dbcontext = dbcontext;
            _mapper = mapper;
            _requestdto = new StripeRequestDTO();
            
        }

        public Task<string> ApplyCoupon(string CouponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ConfirmOrder(Guid OrderId , Guid userId)
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
                    var productstobesold = await _productservice.ProductsToBeSold(products , userId);

                    int? totalcost = 0;


                    productstobesold.ForEach(p =>
                    {
                        totalcost += p.Quantity * (int)p.Price;

                    });





                    var mappedproductstobesold = _mapper.Map<List<ProductSales>>(productstobesold);
                    

                    var sales1 = new Sales(){ 
                        CustomerId = userId,
                        OrderId = OrderId,
                        Products = mappedproductstobesold,
                        TotalCost = (decimal)totalcost
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

        public async Task<List<TransactionsDTO>> CustomerTransactionHistory(Guid userId)
        {
            try {

                var transactions = await _dbcontext.Sales.Where(sales => sales.CustomerId == userId).ToListAsync();

                var mappedtransactions = _mapper.Map<List<TransactionsDTO>>(transactions);



                if(mappedtransactions.Count > 0)
                {
                    return mappedtransactions;
                }
                return null;
            
            } catch (Exception ex) {

                return null;
            }
        }

        public async Task<Sales> GetSaleById(Guid Id)
        {
            try {


                var ordertosold = await _dbcontext.Sales.FindAsync(Id);

                return ordertosold;
            
            } catch(Exception ex)
            {

                return null;
            }
        }

        public async Task<StripeRequestDTO> OrderPurchase(StripeRequestDTO stripeRequest)
        {
            try {

                var sales1 = await GetSaleById(stripeRequest.saleId);


                var options = new SessionCreateOptions()
                {
                    SuccessUrl = stripeRequest.ApprovedUrl,
                    CancelUrl = stripeRequest.CancelUrl,
                    Mode = "payment",
                    LineItems = new List<SessionLineItemOptions>()
                };

                // Getting the Subtotals

                var Item = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)sales1.TotalCost * 100,
                        Currency = "Kes",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = sales1.OrderId.ToString(),
                            Description = "We're Happy Serving You , Welcom!",
                        },

                    },
                    Quantity = 1

                };
                options.LineItems.Add(Item);


                // Working with the Discount

                var discountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon = sales1.CouponCode
                    }
                };

                // Discount will be applied only if it's greater than 0

                if (sales1.Discount > 0)
                {
                    options.Discounts = discountObj;

                }

                var service = new SessionService();

                Session session = service.Create(options);

                _requestdto.StripeSessionUrl = session.Url;
                _requestdto.StripeSessionId = session.Id;

                return _requestdto;


            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UpdateSale()
        {

            await _dbcontext.SaveChangesAsync();
            return "";
            
        }
    }
}
