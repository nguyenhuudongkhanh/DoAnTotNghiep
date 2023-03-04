using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GraduateThesis.WebApi.Controllers;

[Route("api/student-thesis-group")]
[ApiController]
public class ThesisGroupController : ApiControllerBase<IThesisGroupRepository, ThesisGroupInput, ThesisGroupOutput, string>
{
    public ThesisGroupController(IRepository repository, IFileManager fileManager) 
        : base(repository.ThesisGroupRepository, fileManager)
    {
    }

    [Route("batch-delete")]
    [HttpDelete]
    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Create(ThesisGroupInput input)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> ForceDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetDetails([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetPagination(Pagination<ThesisGroupOutput> pagination)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Update([Required] string id, ThesisGroupInput input)
    {
        throw new System.NotImplementedException();
    }
}
