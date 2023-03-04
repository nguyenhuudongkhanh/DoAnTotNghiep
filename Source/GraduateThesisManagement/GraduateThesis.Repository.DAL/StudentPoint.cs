using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class StudentPoint
{
    public string Id { get; set; }

    public string StudentId { get; set; }

    public decimal SupervisorPoint { get; set; }

    public decimal CriticalPoint { get; set; }

    public decimal CommitteePoint { get; set; }

    public virtual Student Student { get; set; }
}
