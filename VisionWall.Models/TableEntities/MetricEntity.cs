using Microsoft.WindowsAzure.Storage.Table;

namespace VisionWall.Models.TableEntities
{
    public class MetricEntity : TableEntity
    {
        public int Value { get; set; }

        public MetricEntity() { }

        public MetricEntity(string metricName, int value)
            : base(metricName, "value")
        {
            Value = value;
        }
    }
}
