using GraduateThesis.ApplicationCore.Enums;
using System.Text.Json.Serialization;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class RecordFilter
{
    public string Key { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ConditionalOperatorOptions ConditionalOperatorOptions { get; set; }

    public object Values { get; set; }

    public RecordFilter Left { get; set; }
    public RecordFilter Right { get; set; } 

    public void AddFilter()
    {

    }

    private void GetFilters()
    {

    }

    public string BuildExpString(string prefix, string value)
    {
        return null;
    }
}
