using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisRevisionInput
{
    [Display(Name = "Mã phiên bản")]
    public string Id { get; set; }

    [Display(Name = "Mã đề tài ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Mã nhóm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string GroupId { get; set; }

    [Display(Name = "Tóm tắt")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Summary { get; set; }

    [Display(Name = "Tiêu đề")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Title { get; set; }

    [Display(Name = "File Doc")]
    [Url(ErrorMessage = "{0} không hợp lệ, nó phải là liên kết")]
    public string DocumentFile { get; set; }

    [Display(Name = "File trình bày")]
    [Url(ErrorMessage = "{0} không hợp lệ, nó phải là liên kết")]
    public string PresentationFile { get; set; }

    [Display(Name = "File PDF")]
    [Url(ErrorMessage = "{0} không hợp lệ, nó phải là liên kết")]
    public string PdfFile { get; set; }

    [Display(Name = "Source code")]
    public string SourceCode { get; set; }

    public bool Reviewed { get; set; }

    [Display(Name = "Nhận xét của giáo viên")]
    public string LecturerComment { get; set; }

    [Display(Name = "Điểm quá trình")]
    public decimal? Point { get; set; }
}

public class ThesisRevisionOutput : ThesisRevisionInput
{
    [Display(Name = "Ngày xét duyệt")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Ngày xóa")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? DeletedAt { get; set; }


    public ThesisOutput Thesis { get; set; }
}
