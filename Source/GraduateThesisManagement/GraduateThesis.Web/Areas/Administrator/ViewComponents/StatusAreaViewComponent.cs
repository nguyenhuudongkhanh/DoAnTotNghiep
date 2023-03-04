using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_StatusArea")]
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
