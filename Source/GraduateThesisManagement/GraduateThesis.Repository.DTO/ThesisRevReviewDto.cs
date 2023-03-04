
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisRevReviewInput
{
    [Display(Name = "Mã phiên bản")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string RevisionId { get; set; }

    [Display(Name = "Nhận xét")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Comment { get; set; }

    [Display(Name = "Điểm quá trình")]
    [Range(0.00, 10.00, ErrorMessage = "{0} có giá trị từ 0.00 đến 10.00")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm không hợp lệ, chỉ chấp nhận 2 chữ số sau dấu phẩy")]
    public decimal? Point { get; set; }
}
