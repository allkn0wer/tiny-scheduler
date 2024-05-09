namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Represents a context for job execution, providing notifications for step lifecycle events.
    /// </summary>
    public interface IJobContext
    {
        /// <summary>
        /// Signals that a step has started execution.
        /// </summary>
        /// <param name="step">The configuration of the step that started.</param>
        void StepStarted(IExecutableStepConfiguration step);

        /// <summary>
        /// Signals that a step has completed successfully.
        /// </summary>
        /// <param name="step">The configuration of the step that completed.</param>
        void StepCompleted(IExecutableStepConfiguration step);

        /// <summary>
        /// Signals that a step has failed during execution.
        /// </summary>
        /// <param name="step">The configuration of the step that failed.</param>
        /// <param name="ex">The exception that caused the failure.</param>
        void StepFailed(IExecutableStepConfiguration step, Exception ex);
    }
}
