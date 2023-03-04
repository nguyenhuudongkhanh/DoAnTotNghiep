using GraduateThesis.ApplicationCore.AppDatabase;
using GraduateThesis.ApplicationCore.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.ApplicationCore.Authorization;

public class PageManager : IPageManager
{
    private AppDbContext _context;

    public PageManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppPage>> GetPagesAsync(string userId)
    {
        List<string> roles = await _context.AppUserRoles.Include(i => i.Role)
            .Where(ur => ur.UserId == userId && ur.Role.IsDeleted == false)
            .Select(s => s.Role.Id).ToListAsync();

        return await _context.AppRoleMappings.Include(i => i.Page)
            .Where(rm => roles.Any(r => r == rm.RoleId) && rm.Page.IsDeleted == false)
            .Select(s => new AppPage { 
                Id = s.Page.Id, 
                PageName = s.Page.PageName,
                ControllerName = s.Page.ControllerName,
                ActionName = s.Page.ActionName,
                Area = s.Page.Area
            }).Distinct().ToListAsync();
    }
}
