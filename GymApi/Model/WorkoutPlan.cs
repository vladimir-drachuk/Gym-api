namespace GymApi.Model
{
    /// <summary>
    /// Represents a workout plan
    /// </summary>
    public class WorkoutPlan
    {
        /// <summary>
        /// Gets or sets the workout plan identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the workout plan description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
