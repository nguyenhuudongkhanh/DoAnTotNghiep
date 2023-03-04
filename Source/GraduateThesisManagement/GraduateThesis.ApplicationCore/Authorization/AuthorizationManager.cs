
using GraduateThesis.ApplicationCore.Context;

namespace GraduateThesis.ApplicationCore.Authorization;

public class AuthorizationManager : IAuthorizationManager
{
    private AppDbContext _context;

    public AuthorizationManager(AppDbContext context)
    {
        _context = context;
    }

    public IAccountManager AccountManager => new AccountManager(_context);

    public IRoleManager RoleManager => new RoleManager(_context);

    public IPageManager PageManager => new PageManager(_context);
}
