using GraduateThesis.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
