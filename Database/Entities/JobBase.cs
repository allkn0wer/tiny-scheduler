using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents a base class for job entities in the database.
    /// </summary>
    public abstract class JobBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the job.
        /// </summary>
        [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the job.
        /// </summary>
        [Required, Column("name"), MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CRON expression defining the schedule of the job.
        /// </summary>
        [Required, Column("cron"), MaxLength(50)]
        public string Cron { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional comment or description of the job.
        /// </summary>
        [Column("comment"), MaxLength(1000)]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the job is active.
        /// </summary>
        [Required, Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
