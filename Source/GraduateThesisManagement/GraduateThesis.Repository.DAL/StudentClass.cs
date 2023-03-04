using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class StudentClass
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int? StudentQuantity { get; set; }

    public string FacultyId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Faculty Faculty { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
