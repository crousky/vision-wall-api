using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using VisionWall.Api.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using VisionWall.Models.Dtos;
using VisionWall.Models.TableEntities;
using System;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

namespace VisionWall.Api.Api
{
    public static class Projects
    {
        [FunctionName("Projects")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "projects/{id?}")]HttpRequestMessage req,
            [Table("projects", Connection = "AzureWebJobsStorage")]CloudTable projectsTable,
            [Table("peopleimpacted", Connection = "AzureWebJobsStorage")]CloudTable peopleImpactedTable,
            [Table("valuecreated", Connection = "AzureWebJobsStorage")]CloudTable valueCreatedTable,
            string id, 
            TraceWriter log)
        {
            log.Info("Projects function processed a request.");

            var tokenHelper = new TokenHelper();
            if (!tokenHelper.AuthorizeUser(req.Headers.Authorization))
            {
                log.Info("user not authorzied");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var projects = new List<Project>();

            var detailQuery = new TableQuery<ProjectDetailEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "detail"));

            if (string.IsNullOrEmpty(id))
            {
                var detailEntities = projectsTable.ExecuteQuery(detailQuery);

                foreach (var detail in detailEntities.OrderByDescending(de => de.CompletionDate))
                {
                    projects.Add(GetProject(detail, peopleImpactedTable, valueCreatedTable));
                }

                return req.CreateResponse(HttpStatusCode.OK, projects, new JsonMediaTypeFormatter
                {
                    SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    },
                    UseDataContractJsonSerializer = false
                });
            }

            detailQuery = detailQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            var projectDetail = projectsTable.ExecuteQuery(detailQuery).FirstOrDefault();

            if (projectDetail == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var project = GetProject(projectDetail, peopleImpactedTable, valueCreatedTable);

            return req.CreateResponse(HttpStatusCode.OK, project, new JsonMediaTypeFormatter
            {
                SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                },
                UseDataContractJsonSerializer = false
            });
        }

        private static Project GetProject(ProjectDetailEntity detail, CloudTable peopleImpactedTable, CloudTable valueCreatedTable)
        {
            var peopleImpactedQuery = new TableQuery<PeopleImpactedEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, detail.PartitionKey));

            var peopleImpactedEntities = peopleImpactedTable.ExecuteQuery(peopleImpactedQuery);

            var externalPeople = peopleImpactedEntities.FirstOrDefault(pi => pi.RowKey == "external");
            var externalPeopleMetric = new Metric(externalPeople.Value, externalPeople.Description);

            var internalPeople = peopleImpactedEntities.FirstOrDefault(pi => pi.RowKey == "internal");
            var internalPeopleMetric = new Metric(internalPeople.Value, internalPeople.Description);

            var valueCreatedQuery = new TableQuery<ValueCreatedEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, detail.PartitionKey));

            var valueCreatedEntities = valueCreatedTable.ExecuteQuery(valueCreatedQuery);

            var costReduction = valueCreatedEntities.FirstOrDefault(vc => vc.RowKey == "costreduction");
            var costReductionMetric = new Metric(costReduction.Value, costReduction.Description);

            var incrementalRevenue = valueCreatedEntities.FirstOrDefault(vc => vc.RowKey == "incrementalrevenue");
            var incrementalRevenueMetric = new Metric(incrementalRevenue.Value, incrementalRevenue.Description);

            return new Project(
                Guid.Parse(detail.PartitionKey),
                detail.ProjectName,
                detail.ClientName,
                detail.CompletionDate,
                detail.SolutionName,
                detail.Description,
                detail.PointOfContact,
                peopleImpactedEntities.Sum(pi => pi.Value),
                externalPeopleMetric,
                internalPeopleMetric,
                valueCreatedEntities.Sum(vc => vc.Value),
                costReductionMetric,
                incrementalRevenueMetric);
        }
    }
}
