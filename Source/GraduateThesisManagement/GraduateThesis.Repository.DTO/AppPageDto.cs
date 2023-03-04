using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class AppPageInput
{
    [Display(Name = "Mã trang")]
    public string Id { get; set; }

    [Display(Name = "Bộ điều khiển")]
    [Required(ErrorMessage = "{0} không được bỏ trống")]
    public string ControllerName { get; set; }

    [Display(Name = "Hành động")]
    [Required(ErrorMessage = "{0} không được bỏ trống")]
    public string ActionName { get; set; }

    [Display(Name = "Khu vực")]
    public string Area { get; set; }

    [Display(Name = "Đường dẫn")]
    public string Path { get; set; }

    [Display(Name = "Tên trang")]
    public string PageName { get; set; }
}

public class AppPageOutput : AppPageInput
{
    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Ngày xóa")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? DeletedAt { get; set; }
}
