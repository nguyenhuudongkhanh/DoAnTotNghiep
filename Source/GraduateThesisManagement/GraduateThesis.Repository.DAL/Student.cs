using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class Student
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public DateTime? Birthday { get; set; }

    public string Avatar { get; set; }

    public string Description { get; set; }

    public string StudentClassId { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string VerificationCode { get; set; }

    public DateTime? CodeExpTime { get; set; }

    public string Gender { get; set; }

    public string Surname { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual StudentClass StudentClass { get; set; }

    public virtual ICollection<ThesisGroupDetail> ThesisGroupDetails { get; } = new List<ThesisGroupDetail>();
}
