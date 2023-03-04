using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_HeadElement")]
public class HeadElementViewComponent : ViewComponent
{
    public HeadElementViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
