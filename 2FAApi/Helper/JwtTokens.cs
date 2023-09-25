using _2FAApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _2FAApi.Helper
{

    //THIS CLASS IS NOT USED. JUST A DEMO
    public class JwtTokens
    {
        //Code to implement JWT TOken. 
        private readonly JwtSettingConfiguration _config;
        private readonly ConcurrentDictionary<string, List<string>> _activeCodes;

        public JwtTokens(IOptions<JwtSettingConfiguration> config)
        {   
            _config = config.Value;
            _activeCodes = new ConcurrentDictionary<string, List<string>>();
        }

        private string GenerateJwtToken(string userId)
        {

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // Add more claims as needed
    };

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.ExpirationMinutes)),
                signingCredentials: credentials
            )
            {

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
