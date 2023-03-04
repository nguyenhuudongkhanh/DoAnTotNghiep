using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_Sidebar")]
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
