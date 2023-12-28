using System.ComponentModel.DataAnnotations;

namespace AuthMS.Data.Dtos
{
    public class AssignRoleDTO
    {
           
        [Required]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
