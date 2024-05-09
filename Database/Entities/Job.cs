using System.ComponentModel.DataAnnotations.Schema;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents a job in the database.
    /// </summary>
    [Table("job")]
    public class Job : JobBase
    {
        /// <summary>
        /// Gets or sets the collection of steps associated with the job.
        /// </summary>
        [InverseProperty(nameof(Step.Job))]
        public IEnumerable<Step>? Steps { get; set; }
    }
}
