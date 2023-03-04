using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public  interface ICouncilRepository : IAsyncSubRepository<CouncilInput, CouncilOutput, string>
{
    Task<byte[]> ExportAsync(string councilId);
}
