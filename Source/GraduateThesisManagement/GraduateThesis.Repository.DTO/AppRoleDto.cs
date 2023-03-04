using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GraduateThesis.Repository.DTO;

public class AppRoleInput
{
    [Display(Name = "Mã chức danh")]
    public string Id { get; set; }

    [Display(Name = "Tên vai trò , tên chức danh")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }
}

public class AppRoleOutput : AppRoleInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<AppPageOutput> AppPages { get; set; }
}
