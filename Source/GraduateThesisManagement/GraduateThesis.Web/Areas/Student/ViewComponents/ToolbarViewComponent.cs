using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_Toolbar")]
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
