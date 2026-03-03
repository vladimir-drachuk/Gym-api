namespace GymApi.Model
{
    /// <summary>
    /// Represents a workout
    /// </summary>
    public class Workout
    {
        /// <summary>
        /// Gets or sets the workout identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the workout plan identifier
        /// </summary>
        public Guid? WorkoutPlanId { get; set; }
        
        /// <summary>
        /// Gets or sets the workout title
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// Gets or sets the workout description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the workout date
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Gets or sets a value indicating whether the workout is completed
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
