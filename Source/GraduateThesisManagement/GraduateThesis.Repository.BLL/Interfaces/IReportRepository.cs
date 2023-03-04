
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IReportRepository
{
    Task<AdminAreaStatsOutput> GetAdminAreaStats();
    Task<FStaffAreaStatsOutput> GetFacultyStaffAreaStats();
    Task<StudentAreaStatsOutput> GetStudentAreaStats();
}
