using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisGroupInput
{
    [Display(Name = "Mã nhóm")]
    public string Id { get; set; }

    [Display(Name = "Tên nhóm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Số lượng sinh viên")]
    public int StudentQuantity { get; set; }

    [Display(Name = "Mô tả")]
    public string Notes { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsFinished { get; set; }
}

public class ThesisGroupOutput : ThesisGroupInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ThesisOutput Thesis { get; set; }

    [Display(Name = "Thành viên")]
    public List<StudentOutput> Students { get; set; }
}