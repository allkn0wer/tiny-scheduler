using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SK.TinyScheduler.Database.SqlServer
{
    public class SchedulerDbContextFactory : IDesignTimeDbContextFactory<SchedulerDbContext>
    {
        public SchedulerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchedulerDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=scheduler;MultipleActiveResultSets=True",
                b => b.MigrationsAssembly("Database.SqlServer")
            );
            return new SchedulerDbContext(optionsBuilder.Options);
        }
    }
}
