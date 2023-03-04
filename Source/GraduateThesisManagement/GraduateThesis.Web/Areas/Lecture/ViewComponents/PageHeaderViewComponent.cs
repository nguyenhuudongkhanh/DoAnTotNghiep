using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_PageHeader")]
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
