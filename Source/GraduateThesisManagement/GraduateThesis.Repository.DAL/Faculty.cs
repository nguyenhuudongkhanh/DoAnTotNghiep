using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class Faculty
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<FacultyStaff> FacultyStaffs { get; } = new List<FacultyStaff>();

    public virtual ICollection<StudentClass> StudentClasses { get; } = new List<StudentClass>();
}
