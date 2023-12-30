using TransactionMS.Data.Dtos;

namespace TransactionMS.Services.Iservice
{
    public interface IProduct
    {
        Task<ProductDTO> GetProductById(Guid Id);

        Task<List<ProductDTO>> ProductsToBeSold(List<OrderProductDTO> OrderProducts);
        Task<string>UpdateProduct(Guid Id,ProductDTO updatedproduct);


    }
}
