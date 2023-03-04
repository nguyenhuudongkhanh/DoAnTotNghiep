using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class CommitteeMemberResult
{
    public string ThesisId { get; set; }

    public string CommitteeMemberId { get; set; }

    public decimal? Point { get; set; }

    public string EvaluationId { get; set; }

    public virtual CommitteeMember CommitteeMember { get; set; }

    public virtual Thesis Thesis { get; set; }
}
