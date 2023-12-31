using AutoMapper;
using CouponMS.Data.Dtos;
using CouponMS.Services.Iservices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace CouponMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        private readonly ICoupon _couponservice;

        private readonly ResponseDTO _response;

        private readonly IMapper _mapper;

        public CouponController(ICoupon couponservice , IMapper mapper)
        {
            _couponservice = couponservice;
            _response = new ResponseDTO();
            _mapper = mapper;
            
        }



        [HttpGet]

        public async Task<ActionResult<ResponseDTO>> GetCouponByCode(string code)
        {

            var coupon = await _couponservice.GetCouponByCouponCode(code);

            if(coupon == null)
            {
                _response.ErrorMessage = "Not Found";
                
                return NotFound(_response);

            }

            _response.Result = coupon;
            return Ok(_response);

        }

        [HttpPost]

        public async Task<ActionResult<ResponseDTO>> CreateCoupon( AddCouponDTO newcoupon) {

            var mappedcoupon = _mapper.Map<Models.Coupon>(newcoupon);

            // stripe coupons setup
            var options = new CouponCreateOptions()
            {
                AmountOff = (long)mappedcoupon.CouponAmount * 100,
                Currency = "KES",
                Id = mappedcoupon.CouponCode,
                Name = mappedcoupon.CouponCode
            };

            var stripeservice = new Stripe.CouponService();
            stripeservice.Create(options);
            var response = await _couponservice.CreateCoupon(mappedcoupon);

            if(response == string.Empty)
            {
                _response.Result = " Coupon Created Successfully";
                return Created("", _response);
            }

            _response.ErrorMessage = "Coupon Not Created Successfully";
            return Ok(_response);


        }
    }
}
