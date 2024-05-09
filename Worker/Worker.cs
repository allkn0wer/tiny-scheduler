using Cronos;
using SK.TinyScheduler.API;
using SK.TinyScheduler.Core;
using SK.TinyScheduler.Core.Extensions;
using SK.TinyScheduler.Database;
using SK.TinyScheduler.Database.Entities;
using System.Collections.Concurrent;
using static SK.TinyScheduler.Core.UpdateStateArgs;

namespace SK.TinyScheduler.Worker
{
    public class Worker : BackgroundService
    {
        private readonly int _cycleTimeout = 1000;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        private DateTimeOffset _lastExecute;

        private readonly ConcurrentDictionary<long, ExecutableJobInstance> _jobInstances = new();

        public Worker(IServiceProvider serviceProvider, IConfiguration config, ILogger<Worker> logger)
        {
            _cycleTimeout = config.GetValue<ushort>("CycleTimeout") * 1000;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _lastExecute = DateTimeOffset.Now;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentIteration = DateTimeOffset.Now;
                _logger.LogTrace("Worker running at: {time}", currentIteration);

                using (var scope = _serviceProvider.CreateScope())
                using (var context = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>())
                {
                    ProcessCancelations(context);
                    ProcessExecutions(context, currentIteration, stoppingToken);
                }

                _lastExecute = currentIteration;
                await Task.Delay(_cycleTimeout, stoppingToken);
            }
        }

        private void ProcessCancelations(SchedulerDbContext context)
        {
            foreach (var instanceId in context.JobInstances.Where(j => j.State == InstanceState.Canceling).Select(i => i.Id))
            {
                try
                {
                    if (_jobInstances.TryRemove(instanceId, out ExecutableJobInstance? executableInstance))
                    {
                        executableInstance.Terminate();
                    }
                    else OnInstanceStateChanged(new UpdateJobStateArgs(instanceId, InstanceState.Failed, new Exception("Job instance lost")));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to cancel job instance {instanceId}");
                }
            }
        }

        private void ProcessExecutions(SchedulerDbContext context, DateTimeOffset currentIteration, CancellationToken stoppingToken)
        {
            foreach (var job in context.Jobs.Where(j => j.IsActive).ToArray())
            {
                try
                {
                    var cronExp = CronExpression.Parse(job.Cron);
                    var nextExecTime = cronExp.GetNextOccurrence(_lastExecute, TimeZoneInfo.Local);
                    var isTimeToGo = nextExecTime.HasValue && nextExecTime < currentIteration;
                    if (isTimeToGo && !IsJobAlreadyRunning(job.Id)) // TODO: implement shared concurrency strategy
                    {
                        _logger.LogDebug($"It's time to run the {job.Name} ({job.Cron})");
                        context.Entry(job).Collection(j => j.Steps!).Load();

                        var jobInstance = job.CreateJobInstance();
                        context.JobInstances.Add(jobInstance);
                        context.SaveChanges();
                        _ = RunJobInstanceAsync(jobInstance, stoppingToken);
                        _logger.LogInformation($"The job {job.Name} was started");
                    }
                }
                catch (CronFormatException cronEx)
                {
                    _logger.LogError(cronEx, $"Invalid cron expression format for job {job.Id}");

                    job.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        private async Task RunJobInstanceAsync(JobInstance jobInstance, CancellationToken stoppingToken)
        {
            ExecutableJobInstance executableInstance = new ExecutableJobInstance(jobInstance, OnInstanceStateChanged);
            _jobInstances.AddOrUpdate(executableInstance.Id, executableInstance, (p, i) => i);
            await executableInstance.ExecuteAsync(stoppingToken);
            _jobInstances.TryRemove(executableInstance.Id, out ExecutableJobInstance? _);
        }

        private bool IsJobAlreadyRunning(long jobId)
        {
            return _jobInstances.ContainsKey(jobId);
        }

        private void OnInstanceStateChanged(UpdateStateArgs args)
        {
            using (var scope = _serviceProvider.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>())
            {
                switch (args)
                {
                    case UpdateJobStateArgs updateJobStateArgs:
                        var jobInstance = context.JobInstances.Find(updateJobStateArgs.Id);
                        if (jobInstance != null)
                        {
                            if (updateJobStateArgs.State == InstanceState.Failed)
                                _logger.LogError(updateJobStateArgs.Error, $"The job {jobInstance.Name} was failed");
                            else 
                                _logger.LogInformation($"The job {jobInstance.Name} state has changed ({jobInstance.State} -> {updateJobStateArgs.State})"); 

                            jobInstance.State = updateJobStateArgs.State;
                            jobInstance.ErrorMessage = updateJobStateArgs.Error?.ToString();
                        }
                        break;
                    case UpdateStepStateArgs updateStepStateArgs:
                        var stepInstance = context.StepInstances.Find(updateStepStateArgs.Id);
                        if (stepInstance != null)
                        {
                            if (updateStepStateArgs.State == InstanceState.Failed)
                                _logger.LogError(updateStepStateArgs.Error, $"The step \"{stepInstance.Name}\" of job \"{stepInstance.JobInstance?.Name}\" was failed");

                            stepInstance.State = updateStepStateArgs.State;
                            stepInstance.ErrorMessage = updateStepStateArgs.Error?.ToString();
                        }
                        break;
                    default: return;
                };
                context.SaveChanges();
            }
        }
    }
}
