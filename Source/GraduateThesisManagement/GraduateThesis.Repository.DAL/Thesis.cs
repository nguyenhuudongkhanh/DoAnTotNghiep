using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class Thesis
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int MaxStudentNumber { get; set; }

    public int Credits { get; set; }

    public string Year { get; set; }

    public string Notes { get; set; }

    public string TopicId { get; set; }

    public string TrainingFormId { get; set; }

    public string TrainingLevelId { get; set; }

    public string SpecializationId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string LectureId { get; set; }

    public int Semester { get; set; }

    public string ThesisGroupId { get; set; }

    public int StatusId { get; set; }

    public string File { get; set; }

    public virtual ICollection<CommitteeMemberResult> CommitteeMemberResults { get; } = new List<CommitteeMemberResult>();

    public virtual CounterArgumentResult CounterArgumentResult { get; set; }

    public virtual ICollection<ImplementationPlan> ImplementationPlans { get; } = new List<ImplementationPlan>();

    public virtual FacultyStaff Lecture { get; set; }

    public virtual Specialization Specialization { get; set; }

    public virtual ThesisStatus Status { get; set; }

    public virtual ThesisCommitteeResult ThesisCommitteeResult { get; set; }

    public virtual ThesisGroup ThesisGroup { get; set; }

    public virtual ICollection<ThesisRevision> ThesisRevisions { get; } = new List<ThesisRevision>();

    public virtual ThesisSupervisor ThesisSupervisor { get; set; }

    public virtual Topic Topic { get; set; }

    public virtual TrainingForm TrainingForm { get; set; }

    public virtual TrainingLevel TrainingLevel { get; set; }
}
