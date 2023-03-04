using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLogLayout_AuthStatusArea")]
public class AuthStatusAreaViewComponent: ViewComponent
{
    public AuthStatusAreaViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
