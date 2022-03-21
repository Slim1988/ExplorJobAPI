using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExplorJobAPI.Auth.Roles;
using Microsoft.IdentityModel.Tokens;

namespace ExplorJobAPI.Domain.Models.Admin
{
    public class AuthTokenAdmin
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }

        public AuthTokenAdmin() {
            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                ),
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    Guid.NewGuid().ToString()
                ),
                new Claim(
                    JwtRegisteredClaimNames.Email,
                    "admin@explorjob.com"
                    
                ),
                new Claim(
                    ClaimTypes.Role,
                    Roles.Administrator
                )
            };

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    Config.AuthSigningSecureKey
                )
            );

            var accessToken = new JwtSecurityToken(
                issuer: Config.AuthIssuer,
                audience: Config.AuthAudience,
                expires: DateTime.Now.AddHours(6),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            AccessToken = new JwtSecurityTokenHandler().WriteToken(
                accessToken
            );

            Expiration = accessToken.ValidTo;
        }
    }
}
