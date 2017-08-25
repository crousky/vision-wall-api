using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace VisionWall.Api.Utilities
{
    public static class HttpHelpers
    {
        public static void AddCorsHeaders(this HttpResponseMessage response, HttpRequestHeaders requestHeaders)
        {
            if (requestHeaders.Contains("Origin"))
            {
                response.Headers.Add("Access-Control-Allow-Credentials", "true");
                response.Headers.Add("Access-Control-Allow-Origin", requestHeaders.GetValues("Origin").FirstOrDefault());
                response.Headers.Add("Access-Control-Allow-Methods", "GET");
            }
        }
    }
}
