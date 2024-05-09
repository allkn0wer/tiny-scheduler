using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SK.TinyScheduler.Database.DependencyInjection
{
    public interface IDbContextConfigurator
    {
        void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration cfg);
    }
}
