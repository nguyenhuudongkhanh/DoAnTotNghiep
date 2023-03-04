using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class AppPage
{
    public string Id { get; set; }

    public string ControllerName { get; set; }

    public string ActionName { get; set; }

    public string Area { get; set; }

    public string Path { get; set; }

    public string PageName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<AppRoleMapping> AppRoleMappings { get; } = new List<AppRoleMapping>();
}
