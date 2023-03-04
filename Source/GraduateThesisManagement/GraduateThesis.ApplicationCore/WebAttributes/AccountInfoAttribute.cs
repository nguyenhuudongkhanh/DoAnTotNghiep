using GraduateThesis.ApplicationCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AccountInfoAttribute : ActionFilterAttribute
{
    private Type _type;

    public AccountInfoAttribute(Type type)
    {
        _type = type;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Controller controller = (filterContext.Controller as Controller);

        IAccountManager accountManager = new AccountManager(null);
        accountManager.SetHttpContext(filterContext.HttpContext);
        controller.ViewData["AccountModel"] = accountManager.GetUser(_type);
    }
}
