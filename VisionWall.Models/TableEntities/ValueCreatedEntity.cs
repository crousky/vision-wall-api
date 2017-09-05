using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class ValueCreatedEntity : TableEntity
    {
        public int Value { get; set; }

        public ValueCreatedEntity() { }

        public ValueCreatedEntity(Guid projectId, string metricName, int value)
            :base(projectId.ToString(), metricName)
        {
            Value = value;
        }
    }
}
