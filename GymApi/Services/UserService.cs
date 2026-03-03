using DataLayer.Entities;
using DataLayer.Interfaces;
using GymApi.Interfaces;
using GymApi.Model;
using GymApi.Utilites;

namespace GymApi.Services
{
    /// <summary>
    /// Service for managing users and authentication
    /// </summary>
    public class UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        TokenProvider tokenProvider
    )
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly TokenProvider _tokenProvider = tokenProvider;

        /// <summary>
        /// Creates a new user with hashed password
        /// </summary>
        /// <param name="user">The user data to create</param>
        /// <returns>The created user</returns>
        public async Task<User> CreateUser(User user)
        {
            var hashedPassword = _passwordHasher.Generate(user.Password);

            var entity = new UserEntity
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

        /// <summary>
        /// Gets a user by email address
        /// </summary>
        /// <param name="email">The user email address</param>
        /// <returns>The user if found, otherwise null</returns>
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

        /// <summary>
        /// Verifies user credentials and generates a token if valid
        /// </summary>
        /// <param name="user">The user to verify</param>
        /// <param name="password">The password to verify</param>
        /// <returns>JWT token if credentials are valid, otherwise null</returns>
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
