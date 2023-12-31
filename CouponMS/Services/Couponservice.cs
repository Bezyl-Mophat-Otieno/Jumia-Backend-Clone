using AutoMapper;
using CouponMS.Data;
using CouponMS.Data.Dtos;
using CouponMS.Models;
using CouponMS.Services.Iservices;
using Microsoft.EntityFrameworkCore;

namespace CouponMS.Services
{
    public class Couponservice : ICoupon
    {

        private readonly IMapper _mapper;

        private readonly ApplicationDBContext _dbcontext;


        public Couponservice(IMapper mapper , ApplicationDBContext dbcontext)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
            
        }
        public async Task<string> CreateCoupon(Coupon newcoupon)
        {
            try {


                await _dbcontext.Coupons.AddAsync(newcoupon);
                await _dbcontext.SaveChangesAsync();

                return "";

            
            
            
            } catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public async Task<Models.Coupon> GetCouponByCouponCode(string couponCode)
        {
            try {
                
                var coupon = await _dbcontext.Coupons.Where(coupon=>coupon.CouponCode == couponCode).FirstOrDefaultAsync();


                return coupon;
            
        
            
            }catch (Exception ex)
            {
                return null;
            }
        }
    }
}
