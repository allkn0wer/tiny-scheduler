using System.Collections.ObjectModel;
using System.Reflection;

namespace SK.TinyScheduler.API
{
    public static class ExecutableStepFactory
    {
        public static readonly IReadOnlyDictionary<string, Type> ExecutableStepTypes;

        static ExecutableStepFactory()
        {
            var dictionary = new Dictionary<string, Type>();
            var stepTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetCustomAttribute<TinySchedulerPluginAttribute>() != null)
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && t.GetCustomAttribute<ExecutableStepTypeAttribute>(false) != null);

            foreach (var stepType in stepTypes)
            {
                var typeName = stepType.GetCustomAttribute<ExecutableStepTypeAttribute>(false)!.Type.ToLower();
                dictionary.Add(typeName, stepType);
            }
            ExecutableStepTypes = new ReadOnlyDictionary<string, Type>(dictionary);
        }

        public static ExecutableStepBase Create(IJobContext context, IExecutableStepConfiguration s)
        {
            if (ExecutableStepTypes.TryGetValue(s.Type.ToLower(), out Type? type))
            {
                var executableStepInstance = Activator.CreateInstance(type, context, s) as ExecutableStepBase;
                if (executableStepInstance == null)
                    throw new InvalidCastException($"Unable to create or cast type {type.FullName} to {nameof(ExecutableStepBase)}");
                return executableStepInstance;
            }
            throw new NotImplementedException($"The executable step type \"{s.Type}\" has not implemented yet");
        }
    }
}
