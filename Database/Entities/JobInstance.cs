using SK.TinyScheduler.API;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents an instance of a job in the database.
    /// </summary>
    [Table("job_instance")]
    public class JobInstance : JobBase
    {
        /// <summary>
        /// Gets or sets the state of the job instance.
        /// </summary>
        [Column("state")]
        public InstanceState State { get; set; } = InstanceState.Pending;

        /// <summary>
        /// Gets or sets the error message associated with the job instance, if any.
        /// </summary>
        [Column("error"), DataType(DataType.Text)]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the collection of step instances associated with the job instance.
        /// </summary>
        public virtual IEnumerable<StepInstance>? Steps { get; set; }
    }
}
