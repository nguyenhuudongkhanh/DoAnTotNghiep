using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class AppRoleMapping
{
    public string RoleId { get; set; }

    public string PageId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppPage Page { get; set; }

    public virtual AppRole Role { get; set; }
}
