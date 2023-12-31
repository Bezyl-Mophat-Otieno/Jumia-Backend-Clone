namespace CouponMS.Data.Dtos
{
    public class ResponseDTO
    {

        public string ErrorMessage { get; set; } = string.Empty;

        public Object Result { get; set; } = default!;

        public bool Issuccess { get; set; } = true;
    }
}
