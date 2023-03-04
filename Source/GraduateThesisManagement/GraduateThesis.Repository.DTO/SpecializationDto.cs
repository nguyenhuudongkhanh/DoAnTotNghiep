using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class SpecializationInput
{
    [Display(Name = "Mã chuyên ngành")]
    public string Id { get; set; }

    [Display(Name = "Tên chuyên ngành")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }
}

public class SpecializationOutput : SpecializationInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
