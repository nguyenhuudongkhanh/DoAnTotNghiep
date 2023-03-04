
namespace GraduateThesis.Repository.DTO;

public class AdminAreaStatsOutput
{
    public string RAM { get; set; }
    public string CPU { get; set; }
    public string Database { get; set; }
}

public class FStaffAreaStatsOutput
{
    public int ThesisNumber { get; set; }
    public int ThesisGroupNumber { get; set; }
    public int FacultyStaffNumber { get; set; }
    public int StudentNumber { get; set; }
}

public class StudentAreaStatsOutput
{

}
