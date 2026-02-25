using DataLayer.Enums;

namespace GymApi.Model
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Email { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
        
        public UserRole Role { get; set; } = UserRole.User;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}