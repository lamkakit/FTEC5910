using FTEC5910.Shared.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Features
{
    public static class JwtFunctions
    {
        public static SigningCredentials GetSigningCredentials(string securityKey)
        {
            var key = Encoding.UTF8.GetBytes(securityKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public static List<Claim> GetClaims(MyIdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return claims;
        }

        public static string GenerateToken(SigningCredentials signingCredentials, List<Claim> claims, string validIssuer, List<string> validAudiences, string expiryInMinutes) 
        {
            validAudiences.ForEach(a => claims.Add(new Claim("aud",a)));

            var tokenOptions = new JwtSecurityToken(
              issuer: validIssuer,
              claims: claims,             
              expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiryInMinutes)),
              signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
