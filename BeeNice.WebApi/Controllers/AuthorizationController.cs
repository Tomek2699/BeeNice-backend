using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BeeNice.Models.Dtos;

namespace BeeNice.WebApi.Controllers
{
    [ApiController]
    [Route("api/Authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthorizationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already in use.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password and confirmation password do not match.");
            }

            var user = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Registration successful.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentException("UserName is required for generating JWT.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken(TokenDto obj)
        {
            var actionResult = ValidateJwtToken(obj.Token);
            return actionResult;
        }

        private IActionResult ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                if (!string.IsNullOrEmpty(token))
                {
                    tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    if (validatedToken is JwtSecurityToken jwtToken)
                    {
                        return Ok("Token is valid!");
                    }
                }

                return Unauthorized("Invalid token format");
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized("Token has expired");
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Token is invalid");
            }
            catch (Exception ex)
            {
                return Unauthorized($"Token validation failed: {ex.Message}");
            }
        }
    }
}
