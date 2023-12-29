using TransactionMS.Data.Dtos;

namespace TransactionMS.Services.Iservice
{
    public interface IProduct
    {
        Task<ProductDTO> GetProductById(Guid Id);


    }
}
