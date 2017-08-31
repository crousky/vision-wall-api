using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;

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

        public bool AuthorizeUser(AuthenticationHeaderValue authHeader)
        {
            if (ConfigurationManager.AppSettings["DisableAuth"] == "true")
            {
                return true;
            }
            if (authHeader == null)
            {
                return false;
            }

            return ValidateToken(authHeader.Parameter);
        }
    }
}
