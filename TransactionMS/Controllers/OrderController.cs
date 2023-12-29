using AutoMapper;
using Azure;
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

        private readonly ResponseDTO _response;

        private readonly IMapper _mapper;

        public OrderController(ApplicationDBContext context, IMapper mapper , IOrder orderservice)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDTO();
            _orderservice = orderservice;

        }

        [HttpPost]

        public async Task<ActionResult<ResponseDTO>> CreateOrder(CreateOrderDTO neworder)
        {

            var mappedOrder = _mapper.Map<Order>(neworder);

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
    }


}
