using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_Toolbar")]
public class ToolbarViewComponent : ViewComponent
{
    public ToolbarViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
