using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jpp.DesignCalculations.Engine.Project
{
    public class ProjectRevision
    {
        public Guid RevisionId { get; set; }

        public string Name { get; set; }

        public string? Status { get; set; }

        public Guid? PreviousRevision { get; set; }

        public bool Issued { get; set; }

        public ProjectRevision(string name)
        {
            RevisionId = Guid.NewGuid();
            Name = name;
            Issued = false;
        }

        public async Task<ProjectRevision> Revise(string name)
        {
            MemoryStream buffer = new MemoryStream();
            await JsonSerializer.SerializeAsync(buffer, this);

            buffer.Position = 0;
            ProjectRevision? newInstance = await JsonSerializer.DeserializeAsync<ProjectRevision>(buffer);
            if(newInstance == null)
                throw new NullReferenceException("Deserialized clone is null");

            newInstance.Name = name;
            newInstance.PreviousRevision = this.RevisionId;
            newInstance.RevisionId = Guid.NewGuid();
            newInstance.Status = String.Empty;
            newInstance.Issued = false;

            return newInstance;
        }
    }
}
