using SK.TinyScheduler.Database.DependencyInjection;
using SK.TinyScheduler.Database;
using SK.TinyScheduler.Core;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Plugins.Enable();

            var builder = WebApplication.CreateBuilder(args);
            var cfg = builder.Configuration;

            // Add services to the container.
            builder.Services.AddDbContext<SchedulerDbContext>(o => DbContextConfigurationHelper.Configure(o, cfg));
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
