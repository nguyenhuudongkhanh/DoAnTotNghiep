using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.ViewComponents
{
    [ViewComponent(Name = "CommonHeader")]
    public class HeaderViewComponent : ViewComponent
    {
        public HeaderViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
