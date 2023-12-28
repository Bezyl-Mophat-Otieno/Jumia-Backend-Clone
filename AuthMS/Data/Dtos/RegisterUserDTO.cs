using System.ComponentModel.DataAnnotations;

namespace AuthMS.Data.Dtos
{
    public class RegisterUserDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public string? Role { get; set; } = "User";



    }
}
