#nullable disable

using GraduateThesis;

namespace GraduateThesis.ApplicationCore.AppDatabase;

public class AppRoleMapping
{
    public string RoleId { get; set; }

    public string PageId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppPage Page { get; set; }

    public virtual AppRole Role { get; set; }
}