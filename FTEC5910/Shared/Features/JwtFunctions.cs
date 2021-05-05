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

        public static List<Claim> GetClaims(MyIdentityUser user, IList<string> roles, List<string> validAudiences)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            validAudiences.ForEach(a => claims.Add(new Claim("aud", a)));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public static string GenerateToken(SigningCredentials signingCredentials, List<Claim> claims, string validIssuer, string expiryInMinutes)
        {
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

            ExtractRolesFromJWT(claims, keyValuePairs);

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

        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');
                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }
    }
}
