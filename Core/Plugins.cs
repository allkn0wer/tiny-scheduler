using System.Collections.ObjectModel;
using System.Reflection;

namespace SK.TinyScheduler.Core
{
    public static class Plugins
    {
        private const string PLUGINS_DIR = "plugins";
        private static IReadOnlyDictionary<string, Assembly> _plugins;

        public static void Enable() 
        {
            var plugins = new Dictionary<string, Assembly>();

            var pluginsDir = Path.GetFullPath(PLUGINS_DIR);
            if (Directory.Exists(pluginsDir))
            {
                foreach (var file in Directory.GetFiles(pluginsDir, "*.dll"))
                {
                    var assembly = Assembly.LoadFile(file);
                    plugins.Add(assembly.GetName().FullName, assembly);
                }
            }
            _plugins = new ReadOnlyDictionary<string, Assembly>(plugins);

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        private static Assembly? ResolveAssembly(object? sender, ResolveEventArgs args)
        {
            if (_plugins.TryGetValue(args.Name, out Assembly? assembly))
            {
                return assembly;
            }
            return null; // Assembly not found
        }
    }
}
