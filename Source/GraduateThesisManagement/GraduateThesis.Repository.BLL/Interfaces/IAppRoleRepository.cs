using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IAppRoleRepository : IAsyncSubRepository<AppRoleInput, AppRoleOutput, string>
{
    Task<DataResponse> AddPageAsync(AppRoleMappingInput input);
    Task<DataResponse> AddPagesAsync(AppRoleMappingInput input);
    Task<DataResponse> RemovePageAsync(AppRoleMappingInput input);
    Task<DataResponse> RemovePagesAsync(AppRoleMappingInput input);
    Task<Pagination<AppRoleOutput>> GetPgnHasNtUserId(string userId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
}