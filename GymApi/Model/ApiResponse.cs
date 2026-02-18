namespace GymApi.Model
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;

        public static ApiResponse Updated(string resource = "Resource") =>
            new() { Message = $"{resource} updated successfully." };
    }
}
