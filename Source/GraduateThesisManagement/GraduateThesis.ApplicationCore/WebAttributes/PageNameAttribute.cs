using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable

namespace GraduateThesis.Common.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class PageNameAttribute : ActionFilterAttribute
{
    public string Name { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Controller controller = (filterContext.Controller as Controller);
        controller.ViewData["PageName"] = Name;
    }
}
