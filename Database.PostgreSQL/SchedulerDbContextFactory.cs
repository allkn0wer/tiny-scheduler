using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SK.TinyScheduler.Database.PostgreSQL
{
    public class SchedulerDbContextFactory : IDesignTimeDbContextFactory<SchedulerDbContext>
    {
        public SchedulerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchedulerDbContext>();
            optionsBuilder.UseNpgsql("Database=scheduler;Username=postgres;Password=postgres",
                b => b.MigrationsAssembly("Database.PostgreSQL")
            );
            return new SchedulerDbContext(optionsBuilder.Options);
        }
    }
}
