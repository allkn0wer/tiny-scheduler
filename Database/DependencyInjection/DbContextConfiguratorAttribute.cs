namespace SK.TinyScheduler.Database.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DbContextConfiguratorAttribute : Attribute
    {
        public DbContextConfiguratorAttribute(string providerName)
        {
            ProviderName = providerName;
        }

        public string ProviderName { get; private set; }
    }
}
