using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SK.TinyScheduler.API;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents an instance of a step in the database.
    /// </summary>
    [Table("step_instance")]
    public class StepInstance : StepBase, IExecutableStepConfiguration
    {
        /// <summary>
        /// Gets or sets the state of the step instance.
        /// </summary>
        [Column("state")]
        public InstanceState State { get; set; } = InstanceState.Pending;

        /// <summary>
        /// Gets or sets the error message associated with the step instance, if any.
        /// </summary>
        [Column("error"), DataType(DataType.Text)]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the collection of child steps associated with this step instance.
        /// </summary>
        [ForeignKey(nameof(ParentStepId))]
        public virtual IEnumerable<IExecutableStepConfiguration>? Steps { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the job instance to which this step instance belongs.
        /// </summary>
        [Required, Column("job_instance_id")]
        public long JobInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the job instance to which this step instance belongs.
        /// </summary>
        [ForeignKey(nameof(JobInstanceId))]
        public virtual JobInstance? JobInstance { get; set; }
    }

}
