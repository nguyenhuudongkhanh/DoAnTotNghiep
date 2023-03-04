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

[Route("api/student")]
[ApiController]
public class StudentController : ApiControllerBase<IStudentRepository, StudentInput, StudentOutput, string>
{
    public StudentController(IRepository repository, IFileManager fileManager) 
        : base(repository.StudentRepository, fileManager)
    {
    }

    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Create(StudentInput input)
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

    public override Task<IActionResult> GetPagination(Pagination<StudentOutput> pagination)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Update([Required] string id, StudentInput input)
    {
        throw new System.NotImplementedException();
    }
}

