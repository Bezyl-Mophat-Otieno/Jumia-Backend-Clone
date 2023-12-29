using Microsoft.EntityFrameworkCore;
using TransactionMS.Models;

namespace TransactionMS.Data
{
    public class ApplicationDBContext:DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options):base(options) { }

        public DbSet<Order> Orders { get; set; }

    }
}
