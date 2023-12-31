using CouponMS.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponMS.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options):base(options) { }

        public DbSet<Coupon> Coupons { get; set; }

    }
}
