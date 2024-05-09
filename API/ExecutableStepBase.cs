namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Represents a base class for executable steps.
    /// </summary>
    public abstract class ExecutableStepBase : IExecutable
    {
        protected readonly IJobContext _context;
        protected readonly IExecutableStepConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutableStepBase"/> class with the specified job context and step configuration.
        /// </summary>
        /// <param name="context">The job context for the executable step.</param>
        /// <param name="configuration">The configuration of the executable step.</param>
        public ExecutableStepBase(IJobContext context, IExecutableStepConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Executes the executable step asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous execution of the step.</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _context.StepStarted(_configuration);
            var retries = _configuration.Retries;
            do
            {
                try
                {
                    await ExecutePayloadAsync(cancellationToken).ConfigureAwait(false);
                    _context.StepCompleted(_configuration);
                    retries = 0;
                }
                catch (OperationCanceledException)
                {
                    retries = 0;
                }
                catch (Exception ex)
                {
                    _context.StepFailed(_configuration, ex);
                    if (_configuration.BreakOnError)
                        throw;
                }
            }
            while (--retries >= 0);
        }

        /// <summary>
        /// Executes the main payload of the executable step asynchronously.
        /// Derived classes must implement this method to define the specific execution logic of the step.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous execution of the step's payload.</returns>
        protected internal abstract Task ExecutePayloadAsync(CancellationToken cancellationToken);
    }
}