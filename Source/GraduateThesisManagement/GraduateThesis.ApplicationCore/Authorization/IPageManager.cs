using GraduateThesis.ApplicationCore.AppDatabase;

namespace GraduateThesis.ApplicationCore.Authorization;

public interface IPageManager
{
    Task<List<AppPage>> GetPagesAsync(string userId);
}
