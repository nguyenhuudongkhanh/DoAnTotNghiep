
namespace GraduateThesis.ApplicationCore.Authorization;

public interface IAuthorizationManager
{
    IAccountManager AccountManager { get; }
    IRoleManager RoleManager { get; }
    IPageManager PageManager { get; }
}
