namespace TransactionMS.Services.Iservice
{
    public interface ISales
    {

        Task<string> CreateSale(Guid OrderId);
    }
}
