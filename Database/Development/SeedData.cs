using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SK.TinyScheduler.Database.Entities;

namespace SK.TinyScheduler.Database.Development
{
    public class SeedData
    {
        public static void Populate(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            if (context.Jobs.Count() > 0)
                return;

            var job1 = new Job
            {
                Name = "Job #1",
                Cron = "*/1 * * * *",
                IsActive = true,
            };
            job1.Steps = new List<Step>
            {
                new Step
                {
                    Name = "Step #1",
                    Type = "cmd",
                    Order = 1,
                    Retries = 0,
                    BreakOnError = true,
                    Script = "echo OK"
                },
                new Step
                {
                    Name = "Step #2",
                    Type = "sql",
                    Order = 2,
                    Retries = 0,
                    BreakOnError = true,
                    Script = "select 1;"
                },
                new Step
                {
                    Name = "Step #3",
                    Type = "sequential",
                    Order = 3,
                    Retries = 0,
                    BreakOnError = true,
                    Steps = new List<Step>
                    {
                        new Step
                        {
                            Name = "Step #3.1",
                            Type = "sql",
                            Order = 1,
                            Retries = 0,
                            BreakOnError = true,
                            Script = "select 31;",
                            Job = job1
                        },
                        new Step
                        {
                            Name = "Step #3.2",
                            Type = "sql",
                            Order = 2,
                            Retries = 0,
                            BreakOnError = true,
                            Script = "select 32;",
                            Job = job1
                        }
                    }
                }
            };

            context.Jobs.Add(job1);
            context.SaveChanges();
        }
    }
}
