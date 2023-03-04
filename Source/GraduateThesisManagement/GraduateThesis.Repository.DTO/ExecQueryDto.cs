using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ExecQueryInput
{
    [Required]
    public string Query { get; set; }
}

public class ExecQueryOutput : ExecQueryInput
{
    public string StringResult { get; set; }
    public object TableResult { get; set; }
}
