namespace TransactionMS.Data.Dtos
{
    public class OrderComprehensiveDTO
    {

        public Guid UserId { get; set; }


        public List<ProductsOrder> Products { get; set; }
    }
}
