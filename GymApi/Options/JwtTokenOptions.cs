namespace GymApi.Options
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
