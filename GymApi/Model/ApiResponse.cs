namespace GymApi.Model
{
    /// <summary>
    /// Represents a standard API response
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Creates a response indicating successful update
        /// </summary>
        /// <param name="resource">The name of the resource that was updated</param>
        /// <returns>An ApiResponse with success message</returns>
        public static ApiResponse Updated(string resource = "Resource") =>
            new() { Message = $"{resource} updated successfully." };
    }
}
