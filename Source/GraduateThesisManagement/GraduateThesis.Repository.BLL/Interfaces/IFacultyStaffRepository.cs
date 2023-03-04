using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IFacultyStaffRepository : IAsyncSubRepository<FacultyStaffInput, FacultyStaffOutput, string>, IAsyncAccountPattern
{
    Task<Pagination<FacultyStaffOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);  
    Task<DataResponse> UpdateProfileAsync(FacultyStaffInput input, FileUploadModel avtUploadModel);
    Task<DataResponse> SetDefaultAvatarAsync(string facultyStaffId);
    Task<byte[]> ExportAsync();
    Task<DataResponse> GrantRoleAsync(AppUserRoleInput input);
    Task<DataResponse> RevokeRoleAsync(AppUserRoleInput input);
}
