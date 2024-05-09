namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Represents a base class for executable step groups that contain nested executable steps.
    /// </summary>
    public abstract class ExecutableStepGroupBase : ExecutableStepBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutableStepGroupBase"/> class with the specified job context and step configuration.
        /// </summary>
        /// <param name="context">The job context for the executable step group.</param>
        /// <param name="stepConfig">The configuration of the executable step group.</param>
        public ExecutableStepGroupBase(IJobContext context, IExecutableStepConfiguration stepConfig)
            : base(context, stepConfig)
        {
            var nestedSteps = stepConfig.Steps ?? Enumerable.Empty<IExecutableStepConfiguration>();
            NestedSteps = nestedSteps.OrderBy(s => s.Order).Select(s => ExecutableStepFactory.Create(context, s)).ToList();
        }

        /// <summary>
        /// Gets the nested executable steps within the executable step group.
        /// </summary>
        public IList<ExecutableStepBase> NestedSteps { get; init; }
    }
}
