using System.ComponentModel.DataAnnotations;

namespace TransactionMS.Data.Dtos
{
    public class ProductsOrder
    {

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
