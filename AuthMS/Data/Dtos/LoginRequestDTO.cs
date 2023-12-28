using System.ComponentModel.DataAnnotations;

namespace AuthMS.Data.Dtos
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
