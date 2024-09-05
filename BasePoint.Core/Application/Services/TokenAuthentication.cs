using BasePoint.Core.Application.Services.Interfaces;
using BasePoint.Core.Shared;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasePoint.Core.Application.Services
{
    public class TokenAuthentication : ITokenAuthentication
    {
        public string GenerateToken(string apiKey, IEnumerable<Claim> claims, TimeSpan? expirationInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var encondingApiKey = Encoding.ASCII.GetBytes(apiKey);

            expirationInMinutes ??= Constants.DefaultApiTokenExpirationInMinutes;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes.Value.TotalMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encondingApiKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string apiKey, TimeSpan? expirationInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var encondingApiKey = Encoding.ASCII.GetBytes(apiKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes.Value.TotalMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encondingApiKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
