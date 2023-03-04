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

[Route("api/topic")]
[ApiController]
public class TopicController : ApiControllerBase<ITopicRepository, TopicInput, TopicOutput, string>
{
    public TopicController(IRepository repository, IFileManager fileManager) 
        : base(repository.TopicRepository, fileManager)
    {
    }

    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    public override async Task<IActionResult> Create(TopicInput input)
    {
        return await CreateResult(input);
    }

    public override async Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        return await ExportResult(exportMetadata);
    }

    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    public override async Task<IActionResult> GetDetails([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    public override async Task<IActionResult> GetPagination(Pagination<TopicOutput> pagination)
    {
        return await GetPaginationResult(pagination);
    }

    public override async Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        return await ImportResult(formFile, importMetadata);
    }

    public override async Task<IActionResult> Update([Required] string id, TopicInput input)
    {
        return await UpdateResult(id, input);
    }
}
