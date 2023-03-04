using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_HeadElement")]
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
