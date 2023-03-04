#nullable disable

using GraduateThesis;

namespace GraduateThesis.ApplicationCore.AppDatabase;

public class AppUserRole
{
    public string UserId { get; set; }

    public string RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppRole Role { get; set; }
}