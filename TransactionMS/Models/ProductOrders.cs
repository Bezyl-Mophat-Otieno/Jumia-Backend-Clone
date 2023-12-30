namespace TransactionMS.Models
{
    public class ProductOrders
    {

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
