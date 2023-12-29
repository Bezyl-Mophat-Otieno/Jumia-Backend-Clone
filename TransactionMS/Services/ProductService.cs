using Newtonsoft.Json;
using TransactionMS.Data.Dtos;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Services
{
    public class ProductService : IProduct
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }
        public async Task<ProductDTO> GetProductById(Guid Id)
        {
            try { 
                var client = _httpClientFactory.CreateClient("Products");
                var response = await client.GetAsync(Id.ToString());
                var content = await response.Content.ReadAsStringAsync();

                var responseDto = JsonConvert.DeserializeObject<ResponseDTO>(content);

                if(responseDto.Result != null && response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductDTO>(responseDto.Result.ToString());
                }

                return null;
            
            }catch (Exception ex) {

                return null;
            
            }
        }
    }
}
