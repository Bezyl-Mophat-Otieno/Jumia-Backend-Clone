using Microsoft.EntityFrameworkCore;
using ProductMS.Models;

namespace ProductMS.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext( DbContextOptions<ApplicationDBContext>options):base(options) { }

        public DbSet<Product> Products { get; set; }

    }
}
