using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : IAsyncSubRepository<StudentInput, StudentOutput, string>, IAsyncAccountPattern
{
    Task<Pagination<StudentOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<DataResponse> UpdateProfileAsync(StudentInput input, FileUploadModel avtUploadModel);
    Task<DataResponse> SetDefaultAvatarAsync(string studentId);
    Task<DataResponse<string>> CheckRegThesisAsync(string studentId);
    Task<object> SearchForThesisRegAsync(string keyword);
    Task<object> GetForThesisRegAsync(string studentId);
    Task<Pagination<StudentOutput>> GetPgnOfUnRegdStdntAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<StudentOutput>> GetPgnOfRegdStdntAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<byte[]> ExportAsync();
    Task<byte[]> ExportUnRegdStdntsAsync();
    Task<byte[]> ExportRegdStdntsAsync();
}
