using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/Lecturers")]
    [ApiController]
    public class LecturersControler : ApiControllerBase<ILecturersRepository, LecturersInput, LecturersOutput, string>
    {
        public LecturersControler(IRepository repository) : base(repository.LecturersRepository)
        {
        }
    }
}

