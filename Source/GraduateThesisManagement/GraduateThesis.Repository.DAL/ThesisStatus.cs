using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisStatus
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Thesis> Theses { get; } = new List<Thesis>();
}
