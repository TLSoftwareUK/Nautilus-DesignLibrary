using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine.Project
{
    public class ProjectConverter : JsonConverter<Project>
    {
        private static JsonSerializerOptions? _options;

        private IUnitConverter _converter;
        
        public ProjectConverter(IUnitConverter converter)
        {
            _converter = converter;
        }
        
        public override Project? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            BuildOptions();
            Project? project = JsonSerializer.Deserialize<Project>(ref reader, _options);
            project.OnDeserialize(_converter);

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
