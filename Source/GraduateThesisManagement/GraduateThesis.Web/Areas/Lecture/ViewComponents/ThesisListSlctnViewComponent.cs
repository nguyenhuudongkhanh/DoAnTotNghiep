using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_ThesisListSlctn")]
public class ThesisListSlctnViewComponent : ViewComponent
{
    public ThesisListSlctnViewComponent()
    {

    }

    public IViewComponentResult Invoke(string action = "Index")
    {
        ViewData["Action"] = action;

        return View();
    }
}