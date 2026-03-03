using DataLayer.Enums;

namespace GymApi.Model
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the user email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user password (hashed)
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user role
        /// </summary>
        public UserRole Role { get; set; } = UserRole.User;
        
        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}