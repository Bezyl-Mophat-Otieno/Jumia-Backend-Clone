using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductMS.Data;
using ProductMS.Data.Dtos;
using ProductMS.Models;
using ProductMS.Services.Iservices;

namespace ProductMS.Services
{
    public class ProductService : IProduct
    {

        private readonly ApplicationDBContext _context;

        private readonly IMapper _mapper;


        public ProductService(ApplicationDBContext context , IMapper mapper)
        {
            
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> AddProduct(Product newproduct)
        {
            try {


                await _context.Products.AddAsync(newproduct);
                await _context.SaveChangesAsync();
                return "";
                
            
            } catch (Exception ex) {

                return ex.Message;
            
            }
        }

        public async Task<string> DeleteProduct(Product product)
        {
            try {

                 _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return "";

            
            
            
            } catch (Exception ex)
            {

                return ex.Message;


            }
        }

        public async Task<List<Product>> GetProducts()
        {

            try {

                var products = await _context.Products.ToListAsync<Product>();
                return products;


            } catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Product> GetSingleProduct(Guid Id)
        {
            try {


                var product = await _context.Products.FindAsync(Id);
                return product;


            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<string> UpdateProduct()
        {

            try {
                await _context.SaveChangesAsync();
                return "";
            }
            catch (Exception ex) {

                return ex.Message;
            }


        }
    }
}
