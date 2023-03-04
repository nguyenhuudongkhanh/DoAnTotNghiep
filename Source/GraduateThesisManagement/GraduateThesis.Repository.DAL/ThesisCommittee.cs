using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisCommittee
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string CouncilId { get; set; }

    public virtual ICollection<CommitteeMember> CommitteeMembers { get; } = new List<CommitteeMember>();

    public virtual Council Council { get; set; }

    public virtual ICollection<ThesisCommitteeResult> ThesisCommitteeResults { get; } = new List<ThesisCommitteeResult>();
}
