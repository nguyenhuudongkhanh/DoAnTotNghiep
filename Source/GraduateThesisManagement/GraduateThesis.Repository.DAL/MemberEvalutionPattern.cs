using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class MemberEvalutionPattern
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal MaxPoint { get; set; }

    public string ParentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<MemberEvaluation> MemberEvaluations { get; } = new List<MemberEvaluation>();
}
