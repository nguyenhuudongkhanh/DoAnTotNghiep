using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_Footer")]
public class FooterViewComponent : ViewComponent
{
    public FooterViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
