using AutoMapper;
using CouponMS.Data.Dtos;
using CouponMS.Models;

namespace CouponMS.Profiles
{
    public class CouponProfiles:Profile
    {
        public CouponProfiles()
        {
            CreateMap<AddCouponDTO , Coupon>().ReverseMap();
        }
    }
}
