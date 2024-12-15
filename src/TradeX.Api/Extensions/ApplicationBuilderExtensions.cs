using Microsoft.EntityFrameworkCore;
using TradeX.Infrastructure.Persistance;

namespace TradeX.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if(dbContext.Database.GetMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
