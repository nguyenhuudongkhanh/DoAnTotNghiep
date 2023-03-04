using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisGroupRepository : IAsyncSubRepository<ThesisGroupInput, ThesisGroupOutput, string>
{
    Task<Pagination<ThesisGroupOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<List<ThesisGroupOutput>> GetListAsync(string studentId);
    Task<ThesisGroupOutput> GetAsync(string studentId, string thesisId);
    Task<List<ThesisGroupDtOutput>> GetGroupsByStdntIdAsync(string studentId);
    Task<DataResponse> JoinToGroupAsync(string studentId, string groupId);
    Task<DataResponse> DenyFromGroupAsync(string studentId, string groupId);
    Task<bool> CheckIsLeaderAsync(string studentId, string groupId);
    Task<List<StudentGroupDtOutput>> GetStndtGrpDtsAsync(string groupId);
    Task<DataResponse> UpdatePointsAsync(GroupPointInput input);
}
