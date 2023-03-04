using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class ForgotPasswordModel : AccountAuthModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
