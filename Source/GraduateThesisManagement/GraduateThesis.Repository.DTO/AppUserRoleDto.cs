using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class AppUserRoleInput
{
    [Display(Name = "Mã người dùng")]
    public string UserId { get; set; }

    [Display(Name = "Mã chức danh")]
    public string RoleId { get; set; }
}
