using System.Text.Json.Serialization;

namespace GraduateThesis.ApplicationCore.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderOptions
{
    ASC, DESC
}
