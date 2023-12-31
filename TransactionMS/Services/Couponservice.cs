using Newtonsoft.Json;
using TransactionMS.Data.Dtos;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Services
{
    public class Couponservice : ICoupon
    {

        private readonly IHttpClientFactory _client;

        public Couponservice(IHttpClientFactory client)
        {
            _client = client;

        }
        public async Task<CouponDTO> GetCouponByCode(string Code)
        {
            try
            {

                var client = _client.CreateClient("Coupons");
                var response = await client.GetAsync($"?code={Code}");
                var content = await response.Content.ReadAsStringAsync();

                var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);

                if(responseDTO != null && response.IsSuccessStatusCode) {


                    return JsonConvert.DeserializeObject<CouponDTO>(responseDTO.Result.ToString());
                }

                return null;

            }
            catch (Exception ex)
            {


                return null;
            }
        }
    }
}
