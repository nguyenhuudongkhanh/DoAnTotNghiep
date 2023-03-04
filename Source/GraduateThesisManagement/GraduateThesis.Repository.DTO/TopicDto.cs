using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class TopicInput
{
    [Display(Name = "Mã chủ đề")]
    public string Id { get; set; }

    [Display(Name = "Tên chủ đề")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }


    [Display(Name = "Mô tả đề tài")]
    public string Description { get; set; }
 
}

public class TopicOutput : TopicInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

}
