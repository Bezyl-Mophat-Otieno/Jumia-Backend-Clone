using TransactionMS.Data.Dtos;
using TransactionMS.Models;

namespace TransactionMS.Services.Iservice
{
    public interface ISales
    {

        Task<string> CreateSale(Guid OrderId , Guid userId);

        Task<List<TransactionsDTO>>CustomerTransactionHistory(Guid userId);
    }
}
