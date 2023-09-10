using System.Security.Claims;
using System.Text.Json;

namespace WebServer.Authentication
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string input)
        {
            var claims = new List<Claim>();
            var payload = input.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            ExtractRoleFromJWT(claims, keyValuePairs);

            claims.AddRange(keyValuePairs.Select(pair => new Claim(pair.Key, pair.Value.ToString())));

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

        private static void ExtractRoleFromJWT(List<Claim> claims, Dictionary<string, object> pairs)
        {
            pairs.TryGetValue("role", out var role);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                pairs.Remove(ClaimTypes.Role);
            }
        }
    }

}