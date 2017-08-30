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
            [Table("metrics", Connection = "AzureWebJobsStorage")]CloudTable metricsTable,
            TraceWriter log)
        {
            log.Info("Value function processed a request.");

            var tokenHelper = new TokenHelper();
            if (!tokenHelper.AuthorizeUser(req.Headers.Authorization))
            {
                log.Info("user not authorzied");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var query = new TableQuery<MetricEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "valuecreated"));

            var valueCreated = metricsTable.ExecuteQuery(query).First();

            var response = req.CreateResponse(HttpStatusCode.OK, valueCreated.Value, "application/json");

            response.AddCorsHeaders(req.Headers);

            return response;
        }
    }
}
