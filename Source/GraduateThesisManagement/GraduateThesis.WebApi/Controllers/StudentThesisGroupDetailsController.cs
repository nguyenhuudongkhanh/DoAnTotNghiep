using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/studentThesisGroupDetails")]
    [ApiController]
    public class StudentThesisGroupDetailsController : ApiControllerBase<IStudentThesisGroupDetailsRepository, StudentThesisGroupDetailsInput, StudentThesisGroupDetailsOutput, string>
    {
        public StudentThesisGroupDetailsController(IRepository repository) : base(repository.StudentThesisGroupDetailsRepository)
        {
        }
    }
}
