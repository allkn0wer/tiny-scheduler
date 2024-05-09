namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Represents the configuration of an executable step.
    /// </summary>
    public interface IExecutableStepConfiguration
    {
        /// <summary>
        /// Gets the unique identifier of the step.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Gets the order of the step within the execution sequence.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets the type of the step.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the name or description of the step.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the execution should break on error for this step.
        /// </summary>
        bool BreakOnError { get; }

        /// <summary>
        /// Gets the number of retries allowed for this step in case of failure.
        /// If set to 0, retries are disabled.
        /// </summary>
        int Retries { get; }

        /// <summary>
        /// Gets the script or action associated with this step.
        /// </summary>
        string Script { get; }

        /// <summary>
        /// Gets the unique identifier of the parent step, if this step is nested within another step.
        /// </summary>
        long? ParentStepId { get; }

        /// <summary>
        /// Gets the collection of child steps, if any, associated with this step.
        /// </summary>
        IEnumerable<IExecutableStepConfiguration>? Steps { get; }
    }
}