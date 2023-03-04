namespace GraduateThesis.ApplicationCore.Models;

#nullable disable

public class AccountSession
{
    public object AccountModel { get; set; }
    public string UserId { get; set; }
    public string StaticRole { get; set; }
    public string Roles { get; set; }
    public DateTime LastSignInTime { get; set; }
}
