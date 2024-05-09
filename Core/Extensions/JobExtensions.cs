using SK.TinyScheduler.API;
using SK.TinyScheduler.Database.Entities;

namespace SK.TinyScheduler.Core.Extensions
{
    public static class JobExtensions
    {
        public static JobInstance CreateJobInstance(this Job job)
        {
            return new JobInstance
            {
                Name = job.Name,
                Comment = job.Comment,
                Cron = job.Cron,
                IsActive = job.IsActive,
                State = InstanceState.Pending,
                Steps = job.Steps?.Select(s => new StepInstance
                {
                    Name = s.Name,
                    Type = s.Type,
                    Order = s.Order,
                    Retries = s.Retries,
                    BreakOnError = s.BreakOnError,
                    ParentStepId = s.ParentStepId,
                    Script = s.Script,
                    State = InstanceState.Pending
                }).ToArray()
            };
        }
    }
}
