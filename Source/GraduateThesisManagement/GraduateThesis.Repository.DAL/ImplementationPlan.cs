using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ImplementationPlan
{
    public string Id { get; set; }

    public string ThesisId { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public string Task { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Thesis Thesis { get; set; }
}
