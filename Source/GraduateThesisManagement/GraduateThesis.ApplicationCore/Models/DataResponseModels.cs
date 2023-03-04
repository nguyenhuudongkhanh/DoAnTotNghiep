using GraduateThesis.ApplicationCore.Enums;
using System.Text.Json.Serialization;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class DataResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataResponseStatus Status { get; set; }
    public string Message { get; set; }
}

public class DataResponse<TModel> : DataResponse
{
    public TModel Data { get; set; }
}

