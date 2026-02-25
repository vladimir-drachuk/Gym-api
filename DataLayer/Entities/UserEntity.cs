using DataLayer.Enums;

namespace DataLayer.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
    }
}
