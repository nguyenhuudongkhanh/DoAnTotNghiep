using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class Pagination<T_DTO>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItemCount { get; set; }
    public string OrderBy { get; set; }
    public OrderOptions OrderOptions { get; set; }
    public RecordFilter Filter { get; set; }
    public string SearchKeyword { get; set; }
    public List<T_DTO> Items { get; set; }
}
