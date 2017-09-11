using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Thinktecture.IdentityModel.Client;
using System;
using System.IdentityModel.Tokens.Jwt;
using VisionWall.Api.Utilities;

namespace VisionWall.Api.Api
{
    public static class Token
    {
        [FunctionName("Token")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (req.Method == HttpMethod.Get)
            {
                var client = new OAuth2Client(new Uri(ConfigurationManager.AppSettings["OneLoginUrl"]));

                string url = client.CreateAuthorizeUrl(
                    clientId: ConfigurationManager.AppSettings["OneLoginClientId"],
                    redirectUri: ConfigurationManager.AppSettings["RedirectUri"],
                    responseType: "id_token",
                    responseMode: "form_post",
                    scope: "openid",
                    nonce: Guid.NewGuid().ToString());

                log.Info("getting token from " + url);

                var response = req.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Location = new Uri(url);
                return response;
            } else if (req.Method == HttpMethod.Post)
            {
                log.Info("token posted");

                var data = await req.Content.ReadAsStringAsync();

                log.Info("token " + data);

                var token = data.Substring(9);

                //todo validation
                //var parameters = new TokenValidationParameters
                //{
                //    key
                //};

                var tokenHandler = new JwtSecurityTokenHandler();

                var unvalidatedJwt = tokenHandler.ReadJwtToken(token);

                //SecurityToken validated;
                //tokenHandler.ValidateToken(token, parameters, out validated);

                //log.Info("token issuer" + validated.Issuer);

                //var jwt = validated as JwtSecurityToken;

                //log.Info("jwt " + jwt.Payload);

                var tokenHelper = new TokenHelper();

                var valid = tokenHelper.ValidateToken(token);

                if (valid)
                {
                    var response = req.CreateResponse(HttpStatusCode.Redirect);
                    response.Headers.Location = new Uri(ConfigurationManager.AppSettings["TokenRedirect"] + token);
                    return response;
                    //return req.CreateResponse(HttpStatusCode.OK, unvalidatedJwt, "application/json");
                } else
                {
                    return req.CreateResponse(HttpStatusCode.Forbidden);
                }
            }

            return req.CreateResponse(HttpStatusCode.OK, "invalid");
        }
    }
}
