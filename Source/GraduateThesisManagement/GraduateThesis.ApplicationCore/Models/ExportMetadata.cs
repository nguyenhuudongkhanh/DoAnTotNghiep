using GraduateThesis.ApplicationCore.Enums;
using System.Text.Json.Serialization;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class ExportMetadata
{
    public string FileName { get; set; }
    public int MaxRecordNumber { get; set; }
    public ExportTypeOptions TypeOptions { get; set; }
    public string SheetName { get; set; }
    public string[] IncludeProperties { get; set; }
}
