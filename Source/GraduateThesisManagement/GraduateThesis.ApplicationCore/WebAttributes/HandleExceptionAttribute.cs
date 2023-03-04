using GraduateThesis.ApplicationCore.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

#nullable disable

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HandleExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        ViewResult viewResult = new ViewResult();
        if (AppDefaultValue.ShowError)
        {
            viewResult.ViewName = "_ErrorWithDetail";
            viewResult.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
            viewResult.ViewData["ErrorMessage"] = context.Exception.Message;
            viewResult.ViewData["StackTrace"] = context.Exception.StackTrace;
        }
        else
        {
            viewResult.ViewName = "_Error";
        }

        context.Result = viewResult;
    }
}
