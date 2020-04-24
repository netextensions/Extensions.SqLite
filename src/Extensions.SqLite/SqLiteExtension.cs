using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetExtensions
{
    public static class SqLiteExtension
    {
        public static IServiceCollection AddSqliteDb<TContext>(this IServiceCollection services, string sqliteDbPath) where TContext : DbContext
        {
            var connectionString = $"Data Source={sqliteDbPath};";
            services.AddDbContext<TContext>(c => c.UseSqlite($"Data Source={connectionString};"));
            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(connectionString)
                .Options;

            using var context = CreateContext(options);
            context.Database.Migrate();

            services.AddSingleton(sp => options);
            return services;
        }

        private static TContext CreateContext<TContext>(DbContextOptions<TContext> options) where TContext : DbContext => (TContext)Activator.CreateInstance(typeof(TContext), options);

    }
}
