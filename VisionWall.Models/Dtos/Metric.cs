namespace VisionWall.Models.Dtos
{
    public class Metric
    {
        public int Value { get; }
        public string Description { get; }

        public Metric(int value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
