using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class ClientTagImageEntity : TableEntity
    {
        public string TagImage { get; set; }

        public ClientTagImageEntity() { }

        public ClientTagImageEntity(Guid clientId, string tagImage)
            :base(clientId.ToString(), "tagimage")
        {
            TagImage = tagImage;
        }
    }
}
