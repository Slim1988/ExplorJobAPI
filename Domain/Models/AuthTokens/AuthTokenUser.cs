using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Domain.Models.AuthUsers
{
    public class AuthTokenUser
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }

        public AuthTokenUser(
            User user
        ) {
            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                ),
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()
                ),
                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email
                ),
                new Claim(
                    ClaimTypes.Role,
                    Roles.User
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
                expires: DateTime.Now.AddDays(2190),
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
