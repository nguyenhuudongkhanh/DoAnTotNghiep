using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/Counterargument")]
    [ApiController]
    public class CounterargumentControler : ApiControllerBase<ICounterargumentRepository, CounterargumentInput, CounterargumentOutput, string>
    {
        public CounterargumentControler(IRepository repository) : base(repository.CounterargumentRepository)
        {
        }
    }
}

