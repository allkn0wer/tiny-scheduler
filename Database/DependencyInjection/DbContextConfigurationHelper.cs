using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SK.TinyScheduler.Database.DependencyInjection
{
    public static class DbContextConfigurationHelper
    {
        public static void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration cfg)
        {
            var providerName = cfg["ProviderName"];
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = SearchForConfiguratorType(assembly, providerName);
                if (type != null)
                {
                    type.Configure(optionsBuilder, cfg);
                    return;
                }
            }
            throw new InvalidOperationException($"No DbContext configurator found for providerName: {providerName}");
        }

        private static IDbContextConfigurator? SearchForConfiguratorType(Assembly assembly, string? providerName)
        {
            var configuratorTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<DbContextConfiguratorAttribute>(false) != null)
                .Where(t => typeof(IDbContextConfigurator).IsAssignableFrom(t))
                .ToArray();

            foreach (var type in configuratorTypes)
            {
                var attribute = type.GetCustomAttribute<DbContextConfiguratorAttribute>();
                if (String.Equals(attribute!.ProviderName, providerName, StringComparison.OrdinalIgnoreCase))
                {
                    return Activator.CreateInstance(type) as IDbContextConfigurator;
                }
            }
            return null;
        }
    }
}
