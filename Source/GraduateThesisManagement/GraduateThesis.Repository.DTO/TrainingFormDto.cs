using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class TrainingFormInput
{
    [Display(Name = "Mã hình thức đào tạo")]
    public string Id { get; set; }


    [Display(Name = "Tên hình thức đào tạo")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

}


public class TrainingFormOutput : TrainingFormInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
