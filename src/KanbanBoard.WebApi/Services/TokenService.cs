using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using KanbanBoard.WebApi.Configurations;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenOptions _tokenOptions;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TokenService(IOptions<JwtTokenOptions> tokenOptions, IDateTimeProvider dateTimeProvider)
        {
            _tokenOptions = tokenOptions.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_tokenOptions.Key);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var identity = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }
            );

            DateTime createdAt = _dateTimeProvider.UtcNow();
            DateTime expires = createdAt.AddSeconds(_tokenOptions.Seconds);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _tokenOptions.Audience,
                Expires = expires,
                Issuer = _tokenOptions.Issuer,
                NotBefore = createdAt,
                SigningCredentials = signingCredentials,
                Subject = identity,
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
