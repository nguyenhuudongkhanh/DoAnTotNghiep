using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class SupvResultInput
{
    [Display(Name = "Mã đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Mã giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string LecturerId { get; set; }

    [Display(Name = "Đánh giá về nội dung")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Contents { get; set; }

    [Display(Name = "Đánh giá về thái độ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Attitudes { get; set; }

    [Display(Name = "Đánh giá về kết quả ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Results { get; set; }

    [Display(Name = "Kết luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Conclusions { get; set; }

    [Display(Name = "Điểm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(0.00, 10.00, ErrorMessage = "{0} có giá trị từ 0.00 đến 10.00")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm không hợp lệ, chỉ chấp nhận 2 chữ số sau dấu phẩy")]
    public decimal? Point { get; set; }
}

public class SupvResultOutput : SupvResultInput
{

}
