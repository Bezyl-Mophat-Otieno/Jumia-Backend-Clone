using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionMS.Data;
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


        private readonly ISales _saleservice;

        private readonly ResponseDTO _response;

        private readonly IMapper _mapper;

        public OrderController(ApplicationDBContext context, IMapper mapper , IOrder orderservice , IProduct productservice , ISales saleservice)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDTO();
            _orderservice = orderservice;
            _productservice = productservice;
            _saleservice = saleservice;

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

        public async Task<ActionResult<ResponseDTO>> UpdateProduct(Guid Id , ProductDTO updatedproduct)
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


        [HttpGet("productstobesod/{OrderId}")]

        public async Task<ActionResult<ResponseDTO>> GetProductsToBeSold(Guid OrderId)
        {

            var order = await _orderservice.GetOrderById(OrderId);

            if (order == null)
            {
                _response.ErrorMessage = "Order Not Found";
                
            }

            var products = order.Products.ToList();

            var productstobesold = await _productservice.ProductsToBeSold(products);

            if(productstobesold != null) {


                _response.Result = productstobesold;
                return Ok(_response);

            }

            _response.ErrorMessage = "Products Not Found";
            return StatusCode(500, _response);

        }

        [HttpPost("purchase/{Id}")]

        public async Task<ActionResult<ResponseDTO>> MakeSell(Guid Id)
        {

            var res =  await _saleservice.CreateSale(Id);

            if(res != string.Empty)
            {
                _response.ErrorMessage = "Something went wrong the Purchase wasn't processed";

                return BadRequest(_response);
            }

            _response.Result = "Product Purchase was successfull";
            return Ok(_response);


        }
    }

}
