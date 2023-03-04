using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FsLogLayout_AuthStatusArea")]
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
