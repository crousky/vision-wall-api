using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using VisionWall.Models.TableEntities;
using System.Threading.Tasks;
using VisionWall.Api.Utilities;

namespace VisionWall.Api.Api
{
    public static class ValueCreated
    {
        [FunctionName("ValueCreated")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "value")]HttpRequestMessage req,
            [Table("valuecreated", Connection = "AzureWebJobsStorage")]CloudTable valueCreatedTable,
            TraceWriter log)
        {
            log.Info("Value function processed a request.");

            var tokenHelper = new TokenHelper();
            if (!tokenHelper.AuthorizeUser(req.Headers.Authorization))
            {
                log.Info("user not authorzied");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var query = new TableQuery<ValueCreatedEntity>();

            var valueCreated = valueCreatedTable.ExecuteQuery(query).Sum(v => v.Value);

            return req.CreateResponse(HttpStatusCode.OK, valueCreated, "application/json");
        }
    }
}
