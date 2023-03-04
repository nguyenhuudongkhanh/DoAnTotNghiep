using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class StudentGroupDtInput
{
    public string StudentId { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsLeader { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }

    [Display(Name = "Điểm GVHD")]
    [Range(0.00, 10.00, ErrorMessage = "{0} có giá trị từ 0.00 đến 10.00")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm không hợp lệ, chỉ chấp nhận 2 chữ số sau dấu phẩy")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public decimal? SupvPoint { get; set; }

    [Display(Name = "Điểm GVPB")]
    [Range(0.00, 10.00, ErrorMessage = "{0} có giá trị từ 0.00 đến 10.00")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm không hợp lệ, chỉ chấp nhận 2 chữ số sau dấu phẩy")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public decimal? CtrArgPoint { get; set; }

    [Display(Name = "Điểm hội đồng")]
    [Range(0.00, 10.00, ErrorMessage = "{0} có giá trị từ 0.00 đến 10.00")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm không hợp lệ, chỉ chấp nhận 2 chữ số sau dấu phẩy")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public decimal? CmtePoint { get; set; }
}

public class StudentGroupDtOutput : StudentGroupDtInput
{
    public string FullName { get { return $"{Surname.Trim(' ')} {Name.Trim(' ')}"; } }
}
