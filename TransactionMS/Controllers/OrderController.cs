using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionMS.Data;
using TransactionMS.Data.Dto;
using TransactionMS.Data.Dtos;
using TransactionMS.Models;
using TransactionMS.Services;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        private readonly IOrder _orderservice;

        private readonly IProduct _productservice;

        private readonly ICoupon _couponservice;

        private readonly ISales _saleservice;

        private readonly ResponseDTO _response;

        private readonly IMapper _mapper;

        public OrderController(ApplicationDBContext context, IMapper mapper , IOrder orderservice , IProduct productservice , ISales saleservice , ICoupon couponservice)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDTO();
            _orderservice = orderservice;
            _productservice = productservice;
            _saleservice = saleservice;
            _couponservice = couponservice;

        }

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<ResponseDTO>> CreateOrder(CreateOrderDTO neworder)
        {

            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                _response.ErrorMessage = "User Must be Authenticated";
                return BadRequest(_response);

            }
            var mappedOrder = _mapper.Map<Order>(neworder);

            // Update the products stock quantity 

            foreach (var product in mappedOrder.Products)
            {


                var existingproduct = await _productservice.GetProductById(product.ProductId);
                existingproduct.Quantity = existingproduct.Quantity - product.Quantity;


                await _productservice.UpdateProduct(product.ProductId, existingproduct);


            }


            var userId = Guid.Parse(User?.Claims.ToList().First().Value);

            mappedOrder.UserId = userId;
            var res = await _orderservice.CreateOrder(mappedOrder);

            if (!string.IsNullOrEmpty(res))
            {
                _response.ErrorMessage = "Order Not Created";
                return StatusCode(500, _response);
            }

            _response.Result = "Order Created";
            return Created("", _response);



        }

        [HttpGet]

        public async Task<ActionResult<ResponseDTO>> GetOrders() {

            var orders = await _orderservice.GetAllOrders();

            if (orders.Count <= 0) {

                _response.ErrorMessage = "Orders Not Found";
                return NotFound(_response);
            }

            _response.Result = orders;

            return Ok(_response);
        
        
        }

        [HttpGet("userorders/{Id}")]

        public async Task<ActionResult<ResponseDTO>> GetUserOrder(Guid Id) {

            var orders = await _orderservice.GetOrderByUserId(Id);

            if(orders.Count <= 0)
            {
                _response.ErrorMessage = "Orders Not Found";
            }

            _response.Result = orders;
        
        return Ok(_response);
        }

    [HttpGet("{Id}")]

    public async Task<ActionResult<ResponseDTO>> GetOrder(Guid Id)
    {

        var order = await _orderservice.GetOrderById(Id) ;

        if (order == null)
        {
            _response.ErrorMessage = "Order Not Found";
        }

            var mappedorder = _mapper.Map<OrderComprehensiveDTO>(order);

        _response.Result = mappedorder;

        return Ok(_response);
    }

        [HttpGet("product/{Id}")]

        public async Task<ActionResult<ResponseDTO>> GetProduct(Guid Id)
        {
            var product = await _productservice.GetProductById(Id);

            if(product == null)
            {
                _response.ErrorMessage = "Product Not Found";
                return NotFound(_response);
            }

            _response.Result = product;

            return Ok(_response);
    }


        [HttpPut("updateproduct/{Id}")]

        public async Task<ActionResult<ResponseDTO>> UpdateProduct(Guid Id , Data.Dtos.ProductOrderDTO updatedproduct)
        {

            var response = await _productservice.UpdateProduct(Id, updatedproduct);

            if (string.IsNullOrEmpty(response))
            {
                _response.Result = "Updated Successfully";
                return Ok(_response);
            }
            _response.ErrorMessage = "Failed to update";
            return StatusCode(500 , _response);
        }


        [HttpGet("ordered-products/{OrderId}")]
        [Authorize]

        public async Task<ActionResult<ResponseDTO>> GetProductsToBeSold(Guid OrderId)
        {

            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                _response.ErrorMessage = "User Must be Authenticated";
                return BadRequest(_response);

            }

            // Get the user Id

            var userId = Guid.Parse(User?.Claims.ToList().First().Value);

            var order = await _orderservice.GetOrderById(OrderId);

            if (order == null)
            {
                _response.ErrorMessage = "Order Not Found";
                
            }



            var products = order.Products.ToList();

            var productstobesold = await _productservice.ProductsToBeSold(products , userId);

            if(productstobesold != null) {


                _response.Result = productstobesold;
                return Ok(_response);

            }

            _response.ErrorMessage = "Products Not Found";
            return StatusCode(500, _response);

        }

        [HttpPost("confirmorder/{Id}")]
        [Authorize]

        public async Task<ActionResult<ResponseDTO>> ConfirmOrder(Guid Id)
        {

            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                _response.ErrorMessage = "User Must be Authenticated";
                return BadRequest(_response);

            }

            // Get the user Id

            var userId = Guid.Parse(User?.Claims.ToList().First().Value);


            var res =  await _saleservice.ConfirmOrder(Id , userId);

            if(res != string.Empty)
            {
                _response.ErrorMessage = "Something went wrong the Order wasn't Confirmed";

                return BadRequest(_response);
            }

            _response.Result = "The Order was  Confirmed was successfull";
            return Ok(_response);


        }


        [HttpGet("transactionhistory/{userId}")]

        public async Task<ActionResult<ResponseDTO>> CustomerTransactionHistory(Guid userId)
        {

            var transactions = await _saleservice.CustomerTransactionHistory(userId);

            if(transactions == null)
            {
                _response.ErrorMessage = "Transaction History N/A";
            }
            _response.Result = transactions;
            return Ok(_response);

        }

        [HttpPut("applyingcoupon/{salesId}")]

        public async Task<ActionResult<ResponseDTO>> ApplyCoupon(string code , Guid salesId)
        {
            // Get Coupon by code

            var coupon =await _couponservice.GetCouponByCode(code);

            if(coupon == null)
            {
                _response.ErrorMessage = "Coupon Code Does Not Exist";
                return NotFound(_response);

            }

            // Update the ordertosold

            var ordertobesold = await _saleservice.GetSaleById(salesId);


            // Update the ordertobesold

            if(ordertobesold == null)
            {
                _response.ErrorMessage = "Order to Sold (Sales) Not Found";
                return NotFound(_response);

            }

            if (ordertobesold.TotalCost < coupon.CouponMinAmount)
            {
                _response.ErrorMessage = "You do not Qualify for a coupon at this time";
                return BadRequest(_response);
            }

            ordertobesold.CouponCode = coupon.CouponCode;
            ordertobesold.Discount = coupon.CouponAmount;

            await _saleservice.UpdateSale();
          return _response;


        }

        [HttpPost("purchase")]
        public async Task<ActionResult<ResponseDTO>> Purchase(StripeRequestDTO striperequest)
        {

            var res = await _saleservice.OrderPurchase(striperequest);
            _response.Result = res;
            return Ok(_response);

        }

        [HttpPost("validate/{salesId}")]


        public async Task<ActionResult<ResponseDTO>> ValidatePayment(Guid salesId)
        {

            var res = await _saleservice.VerifyPayments(salesId);

            if (!res)
            {
                _response.ErrorMessage = "Payment Was'nt made successfully";
                return StatusCode(500,_response);
            }
            _response.Result = res;
            return Ok(_response);

        }
    }

}
