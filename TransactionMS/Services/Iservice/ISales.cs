using TransactionMS.Data.Dto;
using TransactionMS.Data.Dtos;
using TransactionMS.Models;

namespace TransactionMS.Services.Iservice
{
    public interface ISales
    {

        Task<string> ConfirmOrder(Guid OrderId , Guid userId);

        Task<StripeRequestDTO> OrderPurchase(StripeRequestDTO stripeRequest);

        Task<string> UpdateSale();

        Task<Sales> GetSaleById(Guid Id);

        Task<string> ApplyCoupon(string CouponCode);

        Task<List<TransactionsDTO>>CustomerTransactionHistory(Guid userId);
    }
}
