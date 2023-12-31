using CouponMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CouponMS.Extensions
{
    public static class AddMigrations
    {


        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            return app;
        }

    }
}
