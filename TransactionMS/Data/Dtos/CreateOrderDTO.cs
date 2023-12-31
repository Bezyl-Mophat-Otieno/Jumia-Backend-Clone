using System.ComponentModel.DataAnnotations;

namespace TransactionMS.Data.Dtos
{
    public class CreateOrderDTO
    {
        public Guid UserId { get; set; }

        public List<ProductsOrder> Products { get; set; }
    }
}
