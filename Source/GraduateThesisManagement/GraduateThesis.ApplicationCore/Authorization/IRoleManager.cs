using GraduateThesis.ApplicationCore.AppDatabase;

namespace GraduateThesis.ApplicationCore.Authorization;

public interface IRoleManager
{
    Task<bool> IsValidAsync(string userId, string controllerName, string actionName);
    Task<List<AppRole>> GetRolesAsync(string userId);
    Task<List<AppRole>> GetRolesAsync(string controllerName, string actionName);
}
