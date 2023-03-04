using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisGroupDetail
{
    public string StudentThesisGroupId { get; set; }

    public string StudentId { get; set; }

    public string Notes { get; set; }

    public bool IsLeader { get; set; }

    public int StatusId { get; set; }

    public decimal? SupervisorPoint { get; set; }

    public decimal? CtrArgPoint { get; set; }

    public decimal? CommitteePoint { get; set; }

    public decimal? FinalPoint { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual GroupStatus Status { get; set; }

    public virtual Student Student { get; set; }

    public virtual ThesisGroup StudentThesisGroup { get; set; }
}
