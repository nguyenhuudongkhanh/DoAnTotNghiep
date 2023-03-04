#nullable disable

using GraduateThesis;
using GraduateThesis.ApplicationCore.Context;

namespace GraduateThesis.ApplicationCore.AppDatabase;

public class AppRole
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<AppRoleMapping> AppRoleMappings { get; } = new List<AppRoleMapping>();

    public virtual ICollection<AppUserRole> AppUserRoles { get; } = new List<AppUserRole>();
}