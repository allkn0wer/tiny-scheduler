namespace SK.TinyScheduler.Database.Entities
{
    public enum InstanceState
    {
        Pending = 0,
        Executing = 1,
        Canceling = 2,
        Completed = 3,
        Canceled = 4,
        Failed = 5,
    }
}
