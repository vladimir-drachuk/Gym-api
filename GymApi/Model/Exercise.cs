using System.ComponentModel.DataAnnotations;

namespace GymApi.Model
{
    /// <summary>
    /// Represents an exercise
    /// </summary>
    public class Exercise
    {
        /// <summary>
        /// Gets or sets the exercise identifier
        /// </summary>
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the exercise name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the exercise description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
