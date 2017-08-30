using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace VisionWall.Api.Utilities
{
    public class TokenHelper
    {
        public JwtSecurityToken GetTokenFromString(string rawToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(rawToken);
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateToken(string rawToken)
        {
            var token = GetTokenFromString(rawToken);

            if (token == null)
            {
                return false;
            }

            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == "email");

            return emailClaim?.Value.Contains("singlestoneconsulting.com") ?? false;
        }
    }
}
