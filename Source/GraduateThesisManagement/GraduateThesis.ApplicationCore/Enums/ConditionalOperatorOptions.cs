using System.Text.Json.Serialization;

namespace GraduateThesis.ApplicationCore.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConditionalOperatorOptions
{
    And, Or, Not
}
