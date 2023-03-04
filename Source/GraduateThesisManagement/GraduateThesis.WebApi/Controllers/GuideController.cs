using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/Guide")]
    [ApiController]
    public class GuideControler : ApiControllerBase<IGuideRepository, GuideInput, GuideOutput, string>
    {
        public GuideControler(IRepository repository) : base(repository.GuideRepository)
        {
        }
    }
}

