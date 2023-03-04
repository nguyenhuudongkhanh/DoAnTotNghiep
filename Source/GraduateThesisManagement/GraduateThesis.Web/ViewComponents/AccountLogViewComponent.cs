using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.ViewComponents
{
    [ViewComponent(Name = "AccountLog")]
    public class AccountLogComponent : ViewComponent
    {
        public AccountLogComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
