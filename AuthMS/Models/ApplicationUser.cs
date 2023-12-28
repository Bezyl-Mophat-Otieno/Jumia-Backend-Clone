using Microsoft.AspNetCore.Identity;

namespace AuthMS.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
