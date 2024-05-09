using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents a step in the database.
    /// </summary>
    [Table("step")]
    public class Step : StepBase
    {
        /// <summary>
        /// Gets or sets the collection of child steps associated with this step.
        /// </summary>
        [ForeignKey(nameof(ParentStepId))]
        public virtual IEnumerable<Step>? Steps { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the <see cref="Entities.Job"/> to which this step instance belongs.
        /// </summary>
        [Required, Column("job_id")]
        public long JobId { get; set; }

        /// <summary>
        /// Gets or sets the job to which this step belongs.
        /// </summary>
        [ForeignKey(nameof(JobId)), JsonIgnore]
        public virtual Job? Job { get; set; }
    }
}
