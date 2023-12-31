namespace TransactionMS.Data.Dtos
{
    public class ProductOrderDTO
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
