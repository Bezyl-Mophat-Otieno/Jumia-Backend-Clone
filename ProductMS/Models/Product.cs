using System.ComponentModel.DataAnnotations;

namespace ProductMS.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }


        public int? Quantity { get; set; } = 10;

        [Required]
        public decimal Price { get; set; }
    }
}
