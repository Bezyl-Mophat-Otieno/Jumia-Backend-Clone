using AuthMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthMS.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {

        public ApplicationDBContext( DbContextOptions<ApplicationDBContext>options):base(options) { }

        DbSet<ApplicationUser> Users {  get; set; }

    }
}
