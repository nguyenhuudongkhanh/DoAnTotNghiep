using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_StatusArea")]
public class StatusAreaViewComponent : ViewComponent
{
    public StatusAreaViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
