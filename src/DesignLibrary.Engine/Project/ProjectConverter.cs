using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jpp.DesignCalculations.Calculations;

namespace Jpp.DesignCalculations.Engine.Project
{
    public class ProjectConverter : JsonConverter<Project>
    {
        private static JsonSerializerOptions? _options;

        public override Project? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            BuildOptions();
            Project? project = JsonSerializer.Deserialize<Project>(ref reader, _options);
            project.OnDeserialize();
            return project;
        }

        public override void Write(Utf8JsonWriter writer, Project value, JsonSerializerOptions options)
        {
            BuildOptions();
            JsonSerializer.Serialize(writer, value, _options);
        }

        private void BuildOptions()
        {
            if (_options == null)
            {
                _options = new JsonSerializerOptions();
                _options.Converters.Add(new IProjectStandardConverter());
                _options.Converters.Add(new CalculationConverter());
            }
        }
    }
}
