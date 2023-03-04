using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IAppPageRepository : IAsyncSubRepository<AppPageInput, AppPageOutput, string>
{
    Task<Pagination<AppPageOutput>> GetPgnHasRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
    Task<Pagination<AppPageOutput>> GetPgnHasNtRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
}
