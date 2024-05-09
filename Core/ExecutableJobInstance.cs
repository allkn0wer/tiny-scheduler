using SK.TinyScheduler.API;
using SK.TinyScheduler.Database.Entities;

namespace SK.TinyScheduler.Core
{
    public class ExecutableJobInstance : IJobContext
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly JobInstance _jobInstance;
        private readonly Action<UpdateStateArgs> _onStateChangedAction;

        public ExecutableJobInstance(JobInstance jobInstance, Action<UpdateStateArgs> onStateChangedAction)
        {
            _jobInstance = jobInstance;
            _onStateChangedAction = onStateChangedAction;
            var stepInstances = jobInstance.Steps ?? Enumerable.Empty<StepInstance>();

            Steps = stepInstances.Where(s => s.ParentStepId is null)
                                 .OrderBy(s => s.Order)
                                 .Select(s => ExecutableStepFactory.Create(this, s))
                                 .ToList<IExecutable>();

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public long Id => _jobInstance.Id;
        public InstanceState State => _jobInstance.State;
        public IList<IExecutable> Steps { get; set; }

        public Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(_cancellationTokenSource.Cancel);
            return Task.Run(ExecuteCore, _cancellationTokenSource.Token);
        }

        private async Task ExecuteCore()
        {
            NotifyJobStateChanges(InstanceState.Executing);
            try
            {
                foreach (var step in this.Steps)
                {
                    if (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        await step.ExecuteAsync(_cancellationTokenSource.Token);
                    }
                }
                NotifyJobStateChanges(_cancellationTokenSource.IsCancellationRequested ? InstanceState.Canceled : InstanceState.Completed);
            }
            catch (Exception ex)
            {
                NotifyJobStateChanges(InstanceState.Failed, ex);
            }
        }

        public void Terminate()
        {
            _cancellationTokenSource.Cancel();
        }

        private void NotifyJobStateChanges(InstanceState state, Exception? ex = null)
        {
            _onStateChangedAction(new UpdateStateArgs.UpdateJobStateArgs(_jobInstance.Id, state, ex));
        }

        void IJobContext.StepStarted(IExecutableStepConfiguration step)
        {
            _onStateChangedAction(new UpdateStateArgs.UpdateStepStateArgs(step.Id, InstanceState.Executing));
        }

        void IJobContext.StepCompleted(IExecutableStepConfiguration step)
        {
            _onStateChangedAction(new UpdateStateArgs.UpdateStepStateArgs(step.Id, InstanceState.Completed));
        }

        void IJobContext.StepFailed(IExecutableStepConfiguration step, Exception ex)
        {
            _onStateChangedAction(new UpdateStateArgs.UpdateStepStateArgs(step.Id, InstanceState.Failed, ex));
        }
    }

    public record UpdateStateArgs
    {
        public record UpdateJobStateArgs(long Id, InstanceState State, Exception? Error) : UpdateStateArgs;
        public record UpdateStepStateArgs(long Id, InstanceState State, Exception? Error = null) : UpdateStateArgs;
    }
}
