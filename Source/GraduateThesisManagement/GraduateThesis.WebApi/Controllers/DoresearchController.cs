using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/Doresearch")]
    [ApiController]
    public class DoresearchControler : ApiControllerBase<IDoresearchRepository, DoresearchInput, DoresearchOutput, string>
    {
        public DoresearchControler(IRepository repository) : base(repository.DoresearchRepository)
        {
        }
    }
}

