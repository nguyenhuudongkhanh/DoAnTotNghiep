using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisApprovalInput
{
    [Display(Name = "Mã đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Ghi chú")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Notes { get; set; }
}
