using GraduateThesis.ApplicationCore.AppController;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Controllers;

public class AuthorizationController : WebControllerBase
{
    public ActionResult ShowUnauthorize()
    {
        return View();
    }
}
