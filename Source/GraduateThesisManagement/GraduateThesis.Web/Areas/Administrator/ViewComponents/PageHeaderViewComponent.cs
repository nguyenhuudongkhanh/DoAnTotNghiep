using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_PageHeader")]
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
