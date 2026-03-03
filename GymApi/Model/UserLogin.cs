namespace GymApi.Model
{
    /// <summary>
    /// Represents user login credentials
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Gets or sets the user login (email)
        /// </summary>
        public string Login { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
