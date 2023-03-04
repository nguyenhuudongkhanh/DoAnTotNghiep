using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class AppUserRole
{
    public string UserId { get; set; }

    public string RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AppRole Role { get; set; }

    public virtual FacultyStaff User { get; set; }
}
