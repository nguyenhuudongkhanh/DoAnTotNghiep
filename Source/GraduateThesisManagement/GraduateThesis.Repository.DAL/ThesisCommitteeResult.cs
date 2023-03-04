using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisCommitteeResult
{
    public string ThesisId { get; set; }

    public string ThesisCommitteeId { get; set; }

    public string Contents { get; set; }

    public string Conclusions { get; set; }

    public decimal? Point { get; set; }

    public virtual Thesis Thesis { get; set; }

    public virtual ThesisCommittee ThesisCommittee { get; set; }
}
