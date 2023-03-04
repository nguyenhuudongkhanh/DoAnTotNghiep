using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisStatusOutput
{
    public int Id { get; set; }

    [Display(Name = "Trạng thái")]
    public string Name { get; set; }

    public string Description { get; set; }
}
