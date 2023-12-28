using ProductMS.Data.Dtos;
using ProductMS.Models;

namespace ProductMS.Services.Iservices
{
    public interface IProduct
    {
        Task<string> AddProduct(Product newproduct);
        Task<string> UpdateProduct();
        Task<string> DeleteProduct(Product product);
        Task<List<Product>> GetProducts();
        Task<Product> GetSingleProduct(Guid Id);

    }
}
