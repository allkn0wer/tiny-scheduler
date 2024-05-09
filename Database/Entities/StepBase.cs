using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SK.TinyScheduler.Database.Entities
{
    /// <summary>
    /// Represents a base class for step entities.
    /// </summary>
    public abstract class StepBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step.
        /// </summary>
        [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the order of the step within the execution sequence.
        /// </summary>
        [Column("order")]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the type of the step.
        /// </summary>
        [Required, Column("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name or description of the step.
        /// </summary>
        [Required, Column("name"), MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the execution should break on error for this step.
        /// </summary>
        [Column("break_on_error")]
        public bool BreakOnError { get; set; } = true;

        /// <summary>
        /// Gets or sets the number of retries allowed for this step in case of failure.
        /// If set to 0, retries are disabled.
        /// </summary>
        [Required, Column("retries")]
        public int Retries { get; set; } = 0;

        /// <summary>
        /// Gets or sets the script or action associated with this step.
        /// </summary>
        [Column("script"), DataType(DataType.Text)]
        public string Script { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the parent step, if this step is nested within another step.
        /// </summary>
        [Column("parent_step_id")]
        public long? ParentStepId { get; set; }
    }
}
