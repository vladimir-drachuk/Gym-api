using DataLayer.Interfaces;
using GymApi.Interfaces;
using GymApi.Model;
using GymApi.Utilites;

namespace GymApi.Services
{
    public class UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        TokenProvider tokenProvider
    )
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly TokenProvider _tokenProvider = tokenProvider;

        public async Task<User> CreateUser(User user)
        {
            var hashedPassword = _passwordHasher.Generate(user.Password);

            var entity = new DataLayer.Entities.User
            {
                Id = user.Id,
                Name = user.Name,
                PasswordHash = hashedPassword,
                Email = user.Email,
                Role = user.Role,
            };
            await _userRepository.AddAsync(entity);

            return new User
            {
                Id = user.Id,
                Name = user.Name,
                Password = hashedPassword,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var entity = await _userRepository.GetByEmailAsync(email);

            return entity == null ? null : new User
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.PasswordHash,
                Role = entity.Role,
            };
        }

        public string? VerifyUser(User user, string password)
        {
            var isUserValid = _passwordHasher.Verify(password, user.Password);

            if (isUserValid)
            {
                return _tokenProvider.GenerateToken(user);
            }
            
            return null;
        }
    }
}
