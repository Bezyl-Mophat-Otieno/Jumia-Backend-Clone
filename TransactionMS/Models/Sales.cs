using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionMS.Data.Dtos;

namespace TransactionMS.Models
{
    public class Sales
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }


        [Required]
        public List<ProductSales> Products { get; set; }

        [ForeignKey("OrderId")]
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public double TotalCost {  get; set; }

        [Required]
        public DateTime ProcessedAT { get; set; } = DateTime.Now;

    }
}
