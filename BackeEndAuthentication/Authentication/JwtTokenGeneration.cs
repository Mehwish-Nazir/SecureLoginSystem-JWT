using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackeEndAuthentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace BackeEndAuthentication.Authentication
{
    public class JwtTokenGeneration
    {
        private readonly IConfiguration _config;
        public JwtTokenGeneration(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Users user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName?? "User")

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                issuer: _config["jwt:Isuuer"],
                audience: _config["jwt:audience"],
                claims:claims,
                expires:DateTime.UtcNow.AddHours(2),
                signingCredentials:creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
