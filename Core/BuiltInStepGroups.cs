using SK.TinyScheduler.API;

namespace SK.TinyScheduler.Core
{
    [ExecutableStepType("sequential")]
    public class SequentialStepGroup : ExecutableStepGroupBase
    {
        public SequentialStepGroup(IJobContext context, IExecutableStepConfiguration stepConfig) : base(context, stepConfig) { }

        protected override async Task ExecutePayloadAsync(CancellationToken cancellationToken)
        {
            foreach (var step in NestedSteps)
            {
                await step.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    [ExecutableStepType("parallel")]
    public class ParallelStepGroup: ExecutableStepGroupBase
    {
        public ParallelStepGroup(IJobContext context, IExecutableStepConfiguration stepInstance) : base(context, stepInstance) { }

        protected override async Task ExecutePayloadAsync(CancellationToken cancellationToken)
        {
            var tasks = NestedSteps.Select(s => s.ExecuteAsync(cancellationToken));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
