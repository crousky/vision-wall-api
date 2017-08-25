using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using VisionWall.Models.TableEntities;

namespace VisionWall.Api.Api
{
    public static class PeopleImpacted
    {
        [FunctionName("PeopleImpacted")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "impact")]HttpRequestMessage req,
            [Table("metrics", Connection = "AzureWebJobsStorage")]CloudTable metricsTable,
            TraceWriter log)
        {
            log.Info("Impact function processed a request.");

            var query = new TableQuery<MetricEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "peopleimpacted"));

            var valueCreated = metricsTable.ExecuteQuery(query).First();

            return req.CreateResponse(HttpStatusCode.OK, valueCreated.Value, "application/json");
        }
    }
}
