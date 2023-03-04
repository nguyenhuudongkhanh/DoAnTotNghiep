using GraduateThesis.ApplicationCore.AppDatabase;
using GraduateThesis.ApplicationCore.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.ApplicationCore.Authorization;

public class RoleManager : IRoleManager
{
    private readonly AppDbContext _context;

    public RoleManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppRole>> GetRolesAsync(string userId)
    {
        return await _context.AppUserRoles.Include(i => i.Role)
            .Where(ur => ur.UserId == userId && ur.Role.IsDeleted == false)
            .Select(s => new AppRole { Id = s.Role.Id, Name = s.Role.Name }).ToListAsync();
    }

    public async Task<List<AppRole>> GetRolesAsync(string controllerName, string actionName)
    {
        return await _context.AppRoleMappings.Include(i => i.Page).Include(i => i.Role)
            .Where(rm => rm.Page.ControllerName == controllerName && rm.Page.ActionName == actionName && rm.Page.IsDeleted == false && rm.Role.IsDeleted == false)
            .Select(s => new AppRole { Id = s.Role.Id, Name = s.Role.Name }).ToListAsync();
    }

    public async Task<bool> IsValidAsync(string userId, string controllerName, string actionName)
    {
        int count = await _context.AppUserRoles.Where(ur => ur.UserId == userId)
            .Join(
                _context.AppRoles.Where(r => r.IsDeleted == false),
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { UserId = userRole.UserId, RoleId = role.Id }
            ).Join(
                _context.AppRoleMappings,
                combined1 => combined1.RoleId,
                roleMapping => roleMapping.RoleId,
                (combined1, roleMapping) => new { UserId = combined1.UserId, RoleId = combined1.RoleId, PageId = roleMapping.PageId }
            ).Join(
                _context.AppPages.Where(p => p.ControllerName == controllerName && p.ActionName == actionName && p.IsDeleted == false),
                combined2 => combined2.PageId,
                page => page.Id,
                (combined2, page) => new { UserId = combined2.UserId, RoleId = combined2.RoleId, PageId = page.Id }
            ).CountAsync();

        return (count > 0);
    }
}
