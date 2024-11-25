using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeeNice.WebApi.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;

        public JwtTokenMiddleware(RequestDelegate next, string secretKey)
        {
            _next = next;
            _secretKey = secretKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var key = Encoding.ASCII.GetBytes(_secretKey);
                    var principal = ValidateToken(token, key);
                    context.User = principal;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }
            }

            await _next(context);
        }

        private ClaimsPrincipal ValidateToken(string token, byte[] key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        }
    }
}
