using System.Collections.Generic;

namespace GraduateThesis.Repository.DTO;

public class GroupPointInput
{
    public string ThesisId { get; set; }
    public string GroupId { get; set; }
    public List<StudentGroupDtInput> StudentPoints { get; set; }
}
