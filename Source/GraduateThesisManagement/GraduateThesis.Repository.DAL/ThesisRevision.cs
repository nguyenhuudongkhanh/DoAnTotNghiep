using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisRevision
{
    public string Id { get; set; }

    public string ThesisId { get; set; }

    public string Title { get; set; }

    public string Summary { get; set; }

    public string DocumentFile { get; set; }

    public string PresentationFile { get; set; }

    public string PdfFile { get; set; }

    public string SourceCode { get; set; }

    public bool Reviewed { get; set; }

    public string LecturerComment { get; set; }

    public decimal? Point { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Thesis Thesis { get; set; }
}
