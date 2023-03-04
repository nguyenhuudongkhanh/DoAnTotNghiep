using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_PageHeader")]
public class PageHeaderViewComponent : ViewComponent
{
    public PageHeaderViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
