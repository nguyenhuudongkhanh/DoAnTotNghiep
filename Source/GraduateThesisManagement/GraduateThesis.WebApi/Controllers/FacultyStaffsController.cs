using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/facultyStaffs")]
    [ApiController]
    public class FacultyStaffsControler : ApiControllerBase<IFacultyStaffsRepository, FacultyStaffsInput, FacultyStaffsOutput, string>
    {
        public FacultyStaffsControler(IRepository repository) : base(repository.FacultyStaffsRepository)
        {
        }
    }
}

