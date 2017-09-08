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
        public string Description { get; }
        public int PeopleImpacted { get; }
        public Metric ExternalPeopleImpacted { get; }
        public Metric InternalPeopleImpacted { get; }
        public int ValueCreated { get; }
        public Metric CostReduction { get; }
        public Metric IncrementalRevenue { get; }

        public Project(
            Guid id,
            string projectName,
            string clientName,
            DateTime completionDate,
            string solutionName,
            string description,
            int peopleImpacted,
            Metric externalPeopleImpacted,
            Metric internalPeopleImpacted,
            int valueCreated,
            Metric costReduction,
            Metric incrementalRevenue)
        {
            Id = id;
            ProjectName = projectName;
            ClientName = clientName;
            CompletionDate = completionDate;
            SolutionName = solutionName;
            Description = description;
            PeopleImpacted = peopleImpacted;
            ExternalPeopleImpacted = externalPeopleImpacted;
            InternalPeopleImpacted = internalPeopleImpacted;
            ValueCreated = valueCreated;
            CostReduction = costReduction;
            IncrementalRevenue = incrementalRevenue;
        }
    }
}
