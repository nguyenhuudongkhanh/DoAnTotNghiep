using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class CtrArgResultInput
{
    [Display(Name = "Tên đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Tên giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string LecturerId { get; set; }

    [Display(Name = "Đánh giá về nội dung")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Contents { get; set; }

    [Display(Name = "Đánh giá về phương pháp nghiên cứu")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ResearchMethods { get; set; }

    [Display(Name = "Đánh giá về mặt khoa học")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ScientificResults { get; set; }

    [Display(Name = "Đánh giá về mặt thực tiễn")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string PracticalResults { get; set; }

    [Display(Name = "Những thiếu sót")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Defects { get; set; }

    [Display(Name = "Kết luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Conclusions { get; set; }

    [Display(Name = "Câu trả lời")] 
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Answers { get; set; }

    public decimal? Point { get; set; }
}

public class CtrArgResultOutput : CtrArgResultInput
{

}