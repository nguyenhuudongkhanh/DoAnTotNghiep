using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class StudentClassInput
{
    [Display(Name = "Mã lớp học")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Id { get; set; }

    [Display(Name = "Tên lớp")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Số lượng sinh viên của lớp")]
    public int? StudentQuantity { get; set; }

    [Display(Name = "Mã khoa")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string FacultyId { get; set; }
}

public class StudentClassOutput : StudentClassInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public FacultyOutput Faculty { get; set; }
}
