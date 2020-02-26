using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Chatter.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _secretKey;

        public AuthService(IConfiguration configuration)
        {
            _secretKey = configuration["SecretKey"];
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is null or empty");

            var parameters = GetTokenValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, parameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken(Claim[] claims)
        {
            if (claims == null || claims.Length == 0)
                throw new ArgumentException("Claims are not valid");

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is null or empty");

            var parameters = GetTokenValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validateToken = tokenHandler.ValidateToken(token, parameters, out _);
                return validateToken.Claims;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            var key = Encoding.ASCII.GetBytes(_secretKey);
            return new SymmetricSecurityKey(key);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }

    public interface IAuthService
    {
        bool IsTokenValid(string token);
        string GenerateToken(params Claim[] claims);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}