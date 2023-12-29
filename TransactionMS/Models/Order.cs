using System.ComponentModel.DataAnnotations;
using TransactionMS.Data.Dtos;

namespace TransactionMS.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public List<OrderProductDTO> Products { get; set; }

        [Required]

        public DateTime Created { get; set; } = DateTime.Now;


    }
}
