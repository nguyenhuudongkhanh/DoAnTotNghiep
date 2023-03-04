using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class MemberEvaluation
{
    public string CommitteeMemberResultId { get; set; }

    public string EvalutionPatternId { get; set; }

    public string Name { get; set; }

    public decimal Point { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual MemberEvalutionPattern EvalutionPattern { get; set; }
}
