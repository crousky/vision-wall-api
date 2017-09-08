using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class ValueCreatedEntity : TableEntity
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public ValueCreatedEntity() { }

        public ValueCreatedEntity(Guid projectId, string metricName, int value, string description)
            :base(projectId.ToString(), metricName)
        {
            Value = value;
            Description = description;
        }
    }
}
