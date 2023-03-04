using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/research")]
    [ApiController]
    public class ResearchControler : ApiControllerBase<IResearchRepository, ResearchInput, ResearchOutput, string>
    {
        public ResearchControler(IRepository repository) : base(repository.ResearchRepository)
        {
        }
    }
}
