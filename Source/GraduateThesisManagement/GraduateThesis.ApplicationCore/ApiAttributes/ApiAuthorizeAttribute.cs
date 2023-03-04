using Microsoft.AspNetCore.Mvc.Filters;

namespace GraduateThesis.ApplicationCore.ApiAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
    }
}
