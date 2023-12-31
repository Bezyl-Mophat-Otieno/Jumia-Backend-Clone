namespace CouponMS.Models
{
    public class Coupon
    {
        public Guid Id { get; set; }

        public string CouponCode { get; set; }


        public decimal CouponAmount { get; set; }


        public decimal CouponMinAmount { get; set; }
    }
}
