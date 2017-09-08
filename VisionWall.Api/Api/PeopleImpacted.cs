using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using VisionWall.Models.TableEntities;
using VisionWall.Api.Utilities;

namespace VisionWall.Api.Api
{
    public static class PeopleImpacted
    {
        [FunctionName("PeopleImpacted")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "impact")]HttpRequestMessage req,
            [Table("peopleimpacted", Connection = "AzureWebJobsStorage")]CloudTable peopleImpactedTable,
            TraceWriter log)
        {
            log.Info("Impact function processed a request.");

            var tokenHelper = new TokenHelper();
            if (!tokenHelper.AuthorizeUser(req.Headers.Authorization))
            {
                log.Info("user not authorzied");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var query = new TableQuery<PeopleImpactedEntity>();

            var peopleImpacted = peopleImpactedTable.ExecuteQuery(query).Sum(p => p.Value);

            return req.CreateResponse(HttpStatusCode.OK, peopleImpacted, "application/json");
        }
    }
}
