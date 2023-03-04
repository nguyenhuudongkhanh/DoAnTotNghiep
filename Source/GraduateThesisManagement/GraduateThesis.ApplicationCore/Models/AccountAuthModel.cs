using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class AccountAuthModel
{
    public AccountStatus Status { get; set; }
    public string Message { get; set; }
}
