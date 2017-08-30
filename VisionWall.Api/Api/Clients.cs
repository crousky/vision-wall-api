using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using VisionWall.Models.TableEntities;
using VisionWall.Models.Dtos;
using System.Collections.Generic;
using VisionWall.Api.Utilities;
using System;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

namespace VisionWall.Api.Api
{
    public static class Clients
    {
        [FunctionName("Clients")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            [Table("clients", Connection = "AzureWebJobsStorage")]CloudTable clientsTable,
            [Table("clientimages", Connection = "AzureWebJobsStorage")]CloudTable clientImagesTable,
            TraceWriter log)
        {
            log.Info("Clients function processed a request.");

            log.Info("token " + req.Headers.Authorization?.Parameter);

            var tokenHelper = new TokenHelper();
            var token = req.Headers.Authorization?.Parameter;
            if (!tokenHelper.ValidateToken(token))
            {
                log.Info("user not authorzied");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }
            
            var clients = new List<Client>();

            var detailQuery = new TableQuery<ClientDetailEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "detail"));

            var imageQuery = new TableQuery<ClientTagImageEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "tagimage"));

            var detailEntities = clientsTable.ExecuteQuery(detailQuery);

            var imageEntities = clientImagesTable.ExecuteQuery(imageQuery);

            foreach(var detail in detailEntities.OrderBy(de => de.Year).ThenBy(de => de.Month))
            {
                var image = imageEntities.FirstOrDefault(i => i.PartitionKey == detail.PartitionKey);

                clients.Add(new Client(
                    Guid.Parse(detail.PartitionKey),
                    detail.Name,
                    detail.Month,
                    detail.Year,
                    detail.Url,
                    image?.TagImage));
            }

            var response = req.CreateResponse(HttpStatusCode.OK, clients, new JsonMediaTypeFormatter
            {
                SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                },
                UseDataContractJsonSerializer = false
            });

            response.AddCorsHeaders(req.Headers);

            return response;
        }
    }
}
