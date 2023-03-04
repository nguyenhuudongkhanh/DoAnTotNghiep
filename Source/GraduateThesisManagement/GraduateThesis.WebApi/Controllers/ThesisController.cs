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

[Route("api/thesis")]
[ApiController]
public class ThesisController : ApiControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
{
    private IThesisRepository _thesisRepository;

    public ThesisController(IRepository repository, IFileManager fileManager) 
        : base(repository.ThesisRepository, fileManager)
    {
        _thesisRepository = repository.ThesisRepository;
    }

    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Create(ThesisInput input)
    {
        throw new System.NotImplementedException();
    }

    //[Route("do-thesis-register")]
    //[HttpPost]
    //public async Task<IActionResult> DoThesisRegister(ThesisRegisterInput thesisRegisterInput)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        DataResponse dataResponse = await _thesisRepository.DoThesisRegisterAsync(thesisRegisterInput);

    //        return Ok(dataResponse);
    //    }
    //    return Ok(new { Status = "DoAgain" });
    //}

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

    public override Task<IActionResult> GetPagination(Pagination<ThesisOutput> pagination)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Update([Required] string id, ThesisInput input)
    {
        throw new System.NotImplementedException();
    }
}
