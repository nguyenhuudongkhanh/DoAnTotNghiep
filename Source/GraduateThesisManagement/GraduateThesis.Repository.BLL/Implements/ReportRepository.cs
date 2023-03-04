using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ReportRepository : IReportRepository
{
    private HufiGraduateThesisContext _context;

    internal ReportRepository(HufiGraduateThesisContext context)
    {
        _context = context;
    }

    public Task<AdminAreaStatsOutput> GetAdminAreaStats()
    {
        throw new System.NotImplementedException();
    }

    public async Task<FStaffAreaStatsOutput> GetFacultyStaffAreaStats()
    {
        return new FStaffAreaStatsOutput
        {
            ThesisNumber = await _context.Theses.Where(t => t.IsDeleted == false).CountAsync(),
            ThesisGroupNumber = await _context.ThesisGroups.Where(t => t.IsDeleted == false).CountAsync(),
            FacultyStaffNumber = await _context.FacultyStaffs.Where(t => t.IsDeleted == false).CountAsync(),
            StudentNumber = await _context.Students.Where(t => t.IsDeleted == false).CountAsync(),
        };
    }

    public Task<StudentAreaStatsOutput> GetStudentAreaStats()
    {
        throw new System.NotImplementedException();
    }
}
