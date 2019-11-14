using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using apiaccounts.Services.Interfaces;
using apiaccounts.Models;

namespace apiaccounts.Controllers
{
    [Route("v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            if ( ! this.ModelState.IsValid) return this.BadRequest();

            var user = await _authService.Authenticate(login.username, login.password);

            if (user == null) return Unauthorized();

            var token = GenerateAccessToken(user);

            return Ok( new {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid().ToString("N"),
                // refreshToken = _refreshTokenObj.Refreshtoken
                // user = user
            });
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Logout()
        {
            return StatusCode(201, "Logout Successful");
        }

        private JwtSecurityToken GenerateAccessToken(User user) {
            var claims = new[]
            {
                new Claim("user", user.id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:Expiration"])),
                signingCredentials: creds
            );

            return token;
        }
        
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create()){
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}