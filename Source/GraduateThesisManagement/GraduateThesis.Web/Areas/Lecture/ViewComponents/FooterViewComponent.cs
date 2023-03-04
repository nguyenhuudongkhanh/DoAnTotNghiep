using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_Footer")]
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
