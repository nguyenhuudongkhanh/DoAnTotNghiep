using GraduateThesis.ApplicationCore.AppDatabase;
using GraduateThesis.ApplicationCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_Sidebar")]
public class SidebarViewComponent : ViewComponent
{
    private readonly IPageManager _pageManager;
    private readonly IAccountManager _accountManager;

    public SidebarViewComponent(IAuthorizationManager authorizationManager)
    {
        _pageManager = authorizationManager.PageManager;
        _accountManager = authorizationManager.AccountManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _accountManager.SetHttpContext(HttpContext);
        List<AppPage> appPages = await _pageManager.GetPagesAsync(_accountManager.GetSession().UserId);

        return View(appPages);
    }
}
