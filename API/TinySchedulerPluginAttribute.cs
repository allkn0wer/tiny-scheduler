namespace SK.TinyScheduler.API
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class TinySchedulerPluginAttribute : Attribute
    {
        public TinySchedulerPluginAttribute() { }
    }
}
