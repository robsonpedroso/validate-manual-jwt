using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace ValidateManual
{
    class Program
    {
        static string key = "This Is My Secret Key";

        static void Main(string[] args)
        {
            var stringToken = GenerateToken();

            Console.WriteLine($"Token: { stringToken }");

            ValidateToken(stringToken);
        }

        private static string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "Sample",
                audience: "Sample",
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, "Sample")
                },
                expires: DateTime.Now.AddDays(5));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                var test = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: { ex.Message }");
                Console.WriteLine($"Exception StackTrace: { ex.StackTrace }");
                return false;
            }
            
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
            };
        }
    }
}
