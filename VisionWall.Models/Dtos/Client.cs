using System;

namespace VisionWall.Models.Dtos
{
    public class Client
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Month { get; }
        public int Year { get; }
        public string Url { get; }
        public string TagImage { get; }

        public Client(
            Guid id,
            string name,
            int month,
            int year,
            string url,
            string tagImage)
        {
            Id = id;
            Name = name;
            Month = month;
            Year = year;
            Url = url;
            TagImage = tagImage;
        }
    }
}
