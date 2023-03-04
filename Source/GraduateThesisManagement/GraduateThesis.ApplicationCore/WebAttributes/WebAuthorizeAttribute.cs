using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GraduateThesis.ApplicationCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Models;

#nullable disable

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WebAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public WebAuthorizeAttribute() : base()
    {
        
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ControllerActionDescriptor actionDescriptor = (context.ActionDescriptor as ControllerActionDescriptor);

        AppDbContext dbContext = AppDbContext.CreateContext();
        IRoleManager roleManager = new RoleManager(dbContext);
        IAccountManager accountManager = new AccountManager(dbContext);

        accountManager.SetHttpContext(context.HttpContext);
        AccountSession accountSession = accountManager.GetSession();

        if(accountSession == null)
        {
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
            return;
        }

        bool isValid = await roleManager.IsValidAsync(accountSession.UserId, actionDescriptor.ControllerName, actionDescriptor.ActionName);
        if (!isValid)
        {
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
            return;
        }
    }

}
