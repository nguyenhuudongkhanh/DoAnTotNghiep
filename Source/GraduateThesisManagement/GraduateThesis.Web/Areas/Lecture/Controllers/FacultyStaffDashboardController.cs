using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.AppDatabase;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/dashboard")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class FacultyStaffDashboardController : WebControllerBase
{
    private readonly IReportRepository _reportRepository;
    private readonly IAccountManager _accountManager;
    private readonly IPageManager _pageManager;

    public FacultyStaffDashboardController(IRepository repository, IAuthorizationManager authorizationManager)
    {
        _reportRepository = repository.ReportRepository;
        _accountManager = authorizationManager.AccountManager;
        _pageManager = authorizationManager.PageManager;
    }

    [Route("overview")]
    [HttpGet]
    [PageName(Name = "Trang tổng quan")]
    public async Task<IActionResult> Index()
    {
        ViewData["FStaffAreaStats"] = await _reportRepository.GetFacultyStaffAreaStats();

        _accountManager.SetHttpContext(HttpContext);
        ViewData["AppPages"] = await _pageManager.GetPagesAsync(_accountManager.GetUserId());

        return View();
    }
}
