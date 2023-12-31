using TransactionMS.Data.Dtos;

namespace TransactionMS.Services.Iservice
{
    public interface IProduct
    {
        Task<ProductOrderDTO> GetProductById(Guid Id);

        Task<List<ProductOrderDTO>> ProductsToBeSold(List<ProductsOrder> OrderProducts , Guid userId);
        Task<string>UpdateProduct(Guid Id,ProductOrderDTO updatedproduct);


    }
}
