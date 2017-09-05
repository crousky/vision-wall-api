﻿using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VisionWall.Models.TableEntities
{
    public class ProjectDetailEntity : TableEntity
    {
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public DateTime CompletionDate { get; set; }
        public string SolutionName { get; set; }

        public ProjectDetailEntity() { }

        public ProjectDetailEntity(Guid projectId, string projectName, string clientName, DateTime completionDate, string solutionName)
            :base(projectId.ToString(), "detail")
        {
            ProjectName = projectName;
            ClientName = clientName;
            CompletionDate = completionDate;
            SolutionName = solutionName;
        }
    }
}