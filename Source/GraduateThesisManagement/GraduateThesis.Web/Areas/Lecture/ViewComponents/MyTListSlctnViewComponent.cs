using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_MyTListSlctn")]
public class MyTListSlctnViewComponent : ViewComponent
{
    public MyTListSlctnViewComponent()
    {

    }

    public IViewComponentResult Invoke(string action = "Index")
    {
        ViewData["Action"] = action;

        return View();
    }
}