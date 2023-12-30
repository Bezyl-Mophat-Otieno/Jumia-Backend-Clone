using AuthMS.Data.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMS.Data.Dtos;
using ProductMS.Models;
using ProductMS.Services.Iservices;

namespace ProductMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productservice;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _response;


        public ProductController(IProduct producservice ,IMapper mapper)
        {
            _productservice = producservice;
            _mapper = mapper;
            _response = new ResponseDTO();
            
        }



        [HttpPost("add")]
       // [Authorize(Roles = "Admin")]



        public async Task<ActionResult<ResponseDTO>> AddProduct( AddProductDTO newproduct) {


            var mappedproduct = _mapper.Map<Product>(newproduct);

            var res = await _productservice.AddProduct(mappedproduct);

            if(!string.IsNullOrEmpty(res))
            {
                _response.ErrorMessage = "Product Not Added";
                return BadRequest(_response);
            }

            _response.Result = "Product Added Successfully";
            return Created($"/product/{mappedproduct.Id}",_response);
        
       
        
        }


        [HttpGet("{Id}")]

        public async Task<ActionResult<ResponseDTO>> GetProduct(Guid Id)
        {

            var product = await _productservice.GetSingleProduct(Id);

            if(product == null)
            {
                _response.ErrorMessage = "Product Not Found";
                return NotFound(_response);
            }
            _response.Result = product;

            return Ok(_response);


        }


        [HttpPut("update/{Id}")]
        //[Authorize(Roles = "Admin")]



        public async Task<ActionResult<ResponseDTO>> UpdateProduct(Guid Id,AddProductDTO updatedproduct)
        {
            var existingproduct = await _productservice.GetSingleProduct(Id);

            if (existingproduct == null)
            {
                _response.ErrorMessage = "Product Not Found";
                return NotFound(_response);
            }

            var mappedproduct = _mapper.Map(updatedproduct , existingproduct);


          var res =  await _productservice.UpdateProduct();

            if(string.IsNullOrEmpty(res))
            {
                _response.Result = "Product Updated Successfully";
                return Ok(_response);
            }
            else
            {
                _response.ErrorMessage = "Something Went wrong";
                return BadRequest(_response);
            }


            

        }


        [HttpGet]

        public async Task<ActionResult<ResponseDTO>> GetProducts()
        {

            var products = await _productservice.GetProducts();

            if(products.Count > 0)
            {
                _response.Result = products;
                return Ok(_response);
            }

            _response.ErrorMessage = "No products Found";
            return Ok(_response);
        }


        [HttpDelete("{Id}")]
        //[Authorize(Roles = "Admin")]

        public async Task<ActionResult<ResponseDTO>> DeleteProduct(Guid  Id)
        {

            var product = await _productservice.GetSingleProduct(Id);
            if (product == null)
            {
                _response.ErrorMessage = $"Product with {Id} not Found";
                return NotFound(_response);
            }

            var res = await _productservice.DeleteProduct(product);

            if (string.IsNullOrEmpty(res))
            {
            _response.Result = res;
            return Ok(_response);

            }
            _response.ErrorMessage = res;
            return BadRequest(_response);

        }

    }
}
