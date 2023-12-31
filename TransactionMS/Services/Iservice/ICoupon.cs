using TransactionMS.Data.Dtos;

namespace TransactionMS.Services.Iservice
{
    public interface ICoupon
    {
        Task<CouponDTO> GetCouponByCode(string Code);
    }
}
