using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_Paged")]
public class PagedViewComponent : ViewComponent
{
    public PagedViewComponent()
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

