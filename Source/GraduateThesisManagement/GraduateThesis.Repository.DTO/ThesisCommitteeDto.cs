using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisCommitteeInput
{
    [Display(Name = "Mã tiểu ban")]
    public string Id { get; set; }

    [Display(Name = "Tên ủy ban")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Ghi chú ")]
    public string Notes { get; set; }

    [Display(Name = "Mã hội đồng")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string CouncilId { get; set; }
}

public class ThesisCommitteeOutput : ThesisCommitteeInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public CouncilOutput Council { get; set; }
    public List<ThesisOutput> Theses { get; set; }
    public List<CommitteeMemberOutput> Members { get; set; }
}