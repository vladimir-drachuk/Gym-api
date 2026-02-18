
namespace DataLayer.Entities
{
    public class User : BaseModel
    {
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; }  = string.Empty; 

        public string Name { get; set; } = string.Empty;

        public byte Role { get; set; } = 0;
    }
}
