using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/CourseTraining")]
    [ApiController]
    public class CourseTrainingControler : ApiControllerBase<ICourseTrainingRepository, CourseTrainingInput, CourseTrainingOutput, string>
    {
        public CourseTrainingControler(IRepository repository) : base(repository.CourseTrainingRepository)
        {
        }
    }
}

