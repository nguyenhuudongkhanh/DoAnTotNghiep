using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_PagedSearch")]
public class PagedSearchViewComponent : ViewComponent
{
    public PagedSearchViewComponent()
    {

    }

    public IViewComponentResult Invoke(string action = "Index", bool enableOrder = true, bool enableSearchBy = false)
    {
        ViewData["Action"] = action;
        ViewData["EnableOrder"] = enableOrder;
        ViewData["EnableSearchBy"] = enableSearchBy;

        return View();
    }
}
