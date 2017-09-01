using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class ClientDetailEntity : TableEntity
    {
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Url { get; set; }

        public ClientDetailEntity() { }

        public ClientDetailEntity(Guid clientId, string name, int day, int month, int year, string url)
            :base(clientId.ToString(), "detail")
        {
            Name = name;
            Day = day;
            Month = month;
            Year = year;
            Url = url;
        }
    }
}
