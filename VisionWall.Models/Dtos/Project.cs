using System;

namespace VisionWall.Models.Dtos
{
    public class Project
    {
        public Guid Id { get; }
        public string ProjectName { get; }
        public string ClientName { get; }
        public DateTime CompletionDate { get; }
        public string SolutionName { get; }
        public int PeopleImpacted { get; }
        public int ValueCreated { get; }

        public Project(
            Guid id,
            string projectName,
            string clientName,
            DateTime completionDate,
            string solutionName,
            int peopleImpacted,
            int valueCreated)
        {
            Id = id;
            ProjectName = projectName;
            ClientName = clientName;
            CompletionDate = completionDate;
            SolutionName = solutionName;
            PeopleImpacted = peopleImpacted;
            ValueCreated = valueCreated;
        }
    }
}
