using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionMS.Models;

namespace TransactionMS.Data.Dtos
{
    public class UpdateSalesDTO
    {
        public Guid CustomerId { get; set; }

        public Guid OrderId { get; set; }
        public decimal TotalCost { get; set; }

        public decimal Discount { get; set; } = 0;

        public string CouponCode { get; set; } = string.Empty;

    }
}
