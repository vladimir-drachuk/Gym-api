using GymApi.Model;
using GymApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymApi.Utilites
{
    public class TokenProvider(IOptions<JwtTokenOptions> jwtOptions)
    {
        private readonly JwtTokenOptions _jwtOptions = jwtOptions.Value;

        public string GenerateToken(User user)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256              
            );

            var securityToken = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresIn)
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
