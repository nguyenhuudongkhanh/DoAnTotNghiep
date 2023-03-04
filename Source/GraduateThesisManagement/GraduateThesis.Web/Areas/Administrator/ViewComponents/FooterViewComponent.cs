using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_Footer")]
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
