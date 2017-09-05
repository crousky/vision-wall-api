using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class PeopleImpactedEntity : TableEntity
    {
        public int Value { get; set; }

        public PeopleImpactedEntity() { }

        public PeopleImpactedEntity(Guid projectId, string metricName, int value)
            :base(projectId.ToString(), metricName)
        {
            Value = value;
        }
    }
}
