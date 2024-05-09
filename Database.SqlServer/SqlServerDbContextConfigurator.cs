using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SK.TinyScheduler.Database.DependencyInjection;

namespace SK.TinyScheduler.Database.SqlServer
{
    [DbContextConfigurator("SqlServer")]
    public sealed class SqlServerDbContextConfigurator : IDbContextConfigurator
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration cfg)
        {
            var cs = cfg.GetConnectionString("SqlServerDB");
            optionsBuilder.UseSqlServer(cs, b => b.MigrationsAssembly(this.GetType().Assembly.FullName));
        }
    }
}
