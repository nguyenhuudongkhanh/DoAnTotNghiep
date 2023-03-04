using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class AccountVerificationModel : AccountAuthModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string VerificationCode { get; set; }
}
