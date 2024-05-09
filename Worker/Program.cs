using SK.TinyScheduler.Core;
using SK.TinyScheduler.Database;
using SK.TinyScheduler.Database.DependencyInjection;
using SK.TinyScheduler.Database.Development;

namespace SK.TinyScheduler.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Plugins.Enable();

            var builder = Host.CreateApplicationBuilder(args);
            var cfg = builder.Configuration;

            builder.Services.AddDbContext<SchedulerDbContext>(o => DbContextConfigurationHelper.Configure(o, cfg));
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
#if DEBUG
            SeedData.Populate(host.Services);
#endif
            host.Run();
        }
    }
}