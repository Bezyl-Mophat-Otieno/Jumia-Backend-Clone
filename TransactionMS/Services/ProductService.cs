﻿using Newtonsoft.Json;
using System.Text;
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

        public async Task<List<ProductDTO>> ProductsToBeSold(List<OrderProductDTO> OrderProducts)
        {

            List<ProductDTO> productsToBesold = new List<ProductDTO>();
            
            foreach (var orderProduct in OrderProducts)
            {

                var product = await GetProductById(orderProduct.ProductId);
                productsToBesold.Add(product);
            }

            return productsToBesold;



        }

        public async Task<string> UpdateProduct( Guid Id ,ProductDTO updatedproduct)
        {
            try { 

                var client = _httpClientFactory.CreateClient("ProductUpdate");
                var content = new StringContent(JsonConvert.SerializeObject(updatedproduct), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(Id.ToString() , content);
                var responsestring = await response.Content.ReadAsStringAsync();

                var responseDto = JsonConvert.DeserializeObject<ResponseDTO>(responsestring);


                if (responseDto.Result != string.Empty && response.IsSuccessStatusCode)
                {
                    return "";
                }

                return responseDto.ErrorMessage;


            } catch(Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
