using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class SignInModel
{
    [Required]
    [RegularExpression("^[0-9]{10}$")]
    public string Code { get; set; }

    [Required]
    [DataType(DataType.Password)]                
    public string Password { get; set; }
}
