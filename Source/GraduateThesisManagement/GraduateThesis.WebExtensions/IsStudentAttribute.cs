using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

#nullable disable

namespace GraduateThesis.WebExtensions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class IsStudentAttribute : Attribute, IAuthorizationFilter
{
    public IsStudentAttribute()
    {

    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        IAccountManager accountManager = new AccountManager(null);

        accountManager.SetHttpContext(context.HttpContext);
        AccountSession accountSession = accountManager.GetSession();

        if(accountSession == null)
        {
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
            return;
        }

        if (accountSession.Roles != "Student")
        {
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
            return;
        }   
    }
}
