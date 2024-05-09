using Microsoft.EntityFrameworkCore;
using SK.TinyScheduler.Database.Entities;

namespace SK.TinyScheduler.Database
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : base(options) { }

        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Step> Steps => Set<Step>();
        public DbSet<JobInstance> JobInstances => Set<JobInstance>();
        public DbSet<StepInstance> StepInstances => Set<StepInstance>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StepInstance>()
                   .HasMany(s => (IEnumerable<StepInstance>)s.Steps!)
                   .WithOne()
                   .HasForeignKey(s => s.ParentStepId);
        }
    }
}
