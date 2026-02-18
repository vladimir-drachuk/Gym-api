using GymApi.Model;
using GymApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<string>> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new 
                { 
                    message = "User data is required",
                    code = "INVALID_INPUT"
                });
            }

            var existingUser = await _userService.GetUserByEmail(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new 
                { 
                    message = "User already exists",
                    code = "USER_ALREADY_EXISTS"
                });
            }

            var newUser = await _userService.CreateUser(user);
            var token = _userService.VerifyUser(newUser, user.Password);

            if (token == null)
            {
                return BadRequest(new 
                { 
                    message = "Invalid email or password",
                    code = "INVALID_CREDENTIALS"
                });
            }

            return Ok(token);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<string>> Login([FromBody] UserLogin login)
        {
            var user = await _userService.GetUserByEmail(login.Login);
            
            if (user == null)
            {
                return NotFound(new
                {
                    message = "",
                    code = "USER_NOT_FOUND"
                }     
                );
            }
            Console.WriteLine("login");
            var token = _userService.VerifyUser(user, login.Password);

            Console.WriteLine("login token: " + token);
            if (token == null)
            {
                return BadRequest(new
                {
                    message = "Invalid email or password",
                    code = "INVALID_CREDENTIALS"
                });
            }

            return Ok(token);
        }
    }
}
    