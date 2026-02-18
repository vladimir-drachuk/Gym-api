using GymApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GymApi.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtOptions = configuration.GetSection("JwtTokenOptions").Get<JwtTokenOptions>();
            
            if (jwtOptions == null || string.IsNullOrEmpty(jwtOptions.SecretKey))
            {
                throw new InvalidOperationException("JwtTokenOptions is not configured properly");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

            return services;
        }

        public static WebApplication UseJwtAuthentication(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            
            return app;
        }
    }
}
