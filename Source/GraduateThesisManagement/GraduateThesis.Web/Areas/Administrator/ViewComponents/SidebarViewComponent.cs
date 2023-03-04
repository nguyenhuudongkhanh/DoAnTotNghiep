using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_Sidebar")]
public class SidebarViewComponent : ViewComponent
{
    public SidebarViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
