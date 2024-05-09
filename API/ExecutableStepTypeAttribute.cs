namespace SK.TinyScheduler.API
{
    /// <summary>
    /// Specifies the type of an executable step.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ExecutableStepTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutableStepTypeAttribute"/> class with the specified step type.
        /// </summary>
        /// <param name="type">The type of the executable step.</param>
        public ExecutableStepTypeAttribute(string type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type of the executable step specified by this attribute.
        /// </summary>
        public string Type { get; init; }
    }
}
