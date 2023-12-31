using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionMS.Models;

namespace TransactionMS.Data.Dtos
{
    public class TransactionsDTO
    {

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double TotalCost { get; set; }

        public DateTime ProcessedAT { get; set; }
    }
}
