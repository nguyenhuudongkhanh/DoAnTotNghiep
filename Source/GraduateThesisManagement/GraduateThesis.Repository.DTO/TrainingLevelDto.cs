using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO;

public class TrainingLevelInput
{
    [Display(Name = "Mã bậc đào tạo")]
    public string Id { get; set; }

    [Display(Name = "Tên bậc đào tạo")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

}


public class TrainingLevelOutput : TrainingLevelInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
