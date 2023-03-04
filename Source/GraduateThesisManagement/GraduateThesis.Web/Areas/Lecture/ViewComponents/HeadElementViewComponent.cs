using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_HeadElement")]
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
