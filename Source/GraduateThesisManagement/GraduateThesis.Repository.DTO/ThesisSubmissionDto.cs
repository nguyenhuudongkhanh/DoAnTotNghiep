using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisSubmissionInput
{
    public string ThesisId { get; set; }
    public string GroupId { get; set; }

    [Display(Name = "Tập tin")]
    public IFormFile File { get; set; }
}
