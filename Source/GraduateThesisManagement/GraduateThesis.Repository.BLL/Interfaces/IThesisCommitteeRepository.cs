using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisCommitteeRepository : IAsyncSubRepository<ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
    Task<Pagination<ThesisCommitteeOutput>> GetPaginationAsync(string lectureId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<DataResponse> AddThesisAsync(string committeeId, string thesisId);
    Task<DataResponse> DeleteThesisAsync(string committeeId, string thesisId);
    Task<DataResponse> AddMemberAsync(CommitteeMemberInput input);
    Task<DataResponse> DeleteMemberAsync(string committeeId, string memberId);
}
