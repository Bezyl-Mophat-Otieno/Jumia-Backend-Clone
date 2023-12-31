using CouponMS.Data.Dtos;
using CouponMS.Models;

namespace CouponMS.Services.Iservices
{
    public interface ICoupon
    {
        Task<string> CreateCoupon(Coupon newcoupon);
        Task<Coupon> GetCouponByCouponCode(string couponCode);


    }
}
