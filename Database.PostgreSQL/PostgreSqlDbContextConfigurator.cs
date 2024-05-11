using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SK.TinyScheduler.Database.DependencyInjection;

namespace SK.TinyScheduler.Database.PostgreSQL
{
    [DbContextConfigurator("PostgreSQL")]
    public sealed class PostgreSqlDbContextConfigurator : IDbContextConfigurator
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration cfg)
        {
            var cs = cfg.GetConnectionString("PostgreSQL");
            optionsBuilder.UseNpgsql(cs, b => b.MigrationsAssembly(this.GetType().Assembly.FullName));
        }
    }
}
