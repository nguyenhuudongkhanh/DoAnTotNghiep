using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class Topic
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Thesis> Theses { get; } = new List<Thesis>();
}
