using Newtonsoft.Json;
using TransactionMS.Data.Dtos;
using TransactionMS.Services.Iservice;

namespace TransactionMS.Services
{
    public class Userservice : IUser
    {
        private readonly IHttpClientFactory _client;


        public Userservice(IHttpClientFactory client)
        {

            _client = client;

        }
        public async Task<UserDTO> GetUserById(Guid id)
        {
            try {

                var client = _client.CreateClient("Users");
                var response = await client.GetAsync($"{id}");
                var content = await response.Content.ReadAsStringAsync();

                var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);

                if (responseDTO != null && response.IsSuccessStatusCode)
                {


                    return JsonConvert.DeserializeObject<UserDTO>(responseDTO.Result.ToString());
                }

                return null;




            } catch(Exception ex) {


                return null;
            }
        }
    }
}
