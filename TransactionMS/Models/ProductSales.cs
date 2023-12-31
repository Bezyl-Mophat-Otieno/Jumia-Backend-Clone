using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransactionMS.Models
{
    public class ProductSales
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }



        public Guid SalesId { get; set; }
        [ForeignKey("SalesId")]
        [Required]
        public Sales Sales { get; set; }
    }
}
