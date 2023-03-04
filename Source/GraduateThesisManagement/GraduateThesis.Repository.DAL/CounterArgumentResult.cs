using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class CounterArgumentResult
{
    public string ThesisId { get; set; }

    public string LecturerId { get; set; }

    public string Contents { get; set; }

    public string ResearchMethods { get; set; }

    public string ScientificResults { get; set; }

    public string PracticalResults { get; set; }

    public string Defects { get; set; }

    public string Conclusions { get; set; }

    public string Answers { get; set; }

    public decimal? Point { get; set; }

    public bool IsCompleted { get; set; }

    public virtual FacultyStaff Lecturer { get; set; }

    public virtual Thesis Thesis { get; set; }
}
