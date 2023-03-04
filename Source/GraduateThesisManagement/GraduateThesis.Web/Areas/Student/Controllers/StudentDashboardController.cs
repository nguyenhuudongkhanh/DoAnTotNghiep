using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.Controllers;

[Area("Student")]
[Route("student/dashboard")]
[IsStudent]
public class StudentDashboardController : Controller
{
    private IAccountManager _accountManager;

    public StudentDashboardController(IAuthorizationManager authorizationManager)
    {
        _accountManager = authorizationManager.AccountManager;
    }

    [Route("overview")]
    public IActionResult Index()
    {
        return View();
    }
}
