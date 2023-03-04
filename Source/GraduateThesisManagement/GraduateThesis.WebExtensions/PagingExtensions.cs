using GraduateThesis.ApplicationCore.Models;
using X.PagedList;

namespace GraduateThesis.WebExtensions;

public static class PagingExtensions
{
    public static StaticPagedList<T_DTO> ToStaticPagedList<T_DTO>(this Pagination<T_DTO> pagination)
    {
        return new StaticPagedList<T_DTO>(pagination.Items, pagination.Page, pagination.PageSize, pagination.TotalItemCount);
    }
}
