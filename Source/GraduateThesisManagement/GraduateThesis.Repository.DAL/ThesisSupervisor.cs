using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisSupervisor
{
    public string ThesisId { get; set; }

    public string LecturerId { get; set; }

    public string Contents { get; set; }

    public string Attitudes { get; set; }

    public string Results { get; set; }

    public string Conclusions { get; set; }

    public bool IsCompleted { get; set; }

    public decimal? Point { get; set; }

    public virtual FacultyStaff Lecturer { get; set; }

    public virtual Thesis Thesis { get; set; }
}
