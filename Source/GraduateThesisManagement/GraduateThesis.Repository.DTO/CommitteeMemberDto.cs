using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class CommitteeMemberInput
{
    [Display(Name = "Mã")]
    public string Id { get; set; }

    [Display(Name = "Mã tiểu ban")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string CommitteeId { get; set; }

    [Display(Name = "Mã thành viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string MemberId { get; set; }

    [Display(Name = "Chức danh")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Titles { get; set; }
}

public class CommitteeMemberOutput
{
    [Display(Name = " Thành viên")]
    public string MemberId { get; set; }

    [Display(Name = "Họ giảng viên")]
    public string Surname { get; set; }

    [Display(Name = "Tên giảng viên")]
    public string Name { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Chức danh")]
    public string Titles { get; set; }

    [Display(Name = "Tên giảng viên")]
    public string FullName { get { return $"{Surname.Trim(' ')} {Name.Trim(' ')}"; } }
}
