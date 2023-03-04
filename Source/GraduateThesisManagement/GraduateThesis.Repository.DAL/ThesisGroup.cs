using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisGroup
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int StudentQuantity { get; set; }

    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Thesis Thesis { get; set; }

    public virtual ICollection<ThesisGroupDetail> ThesisGroupDetails { get; } = new List<ThesisGroupDetail>();
}
