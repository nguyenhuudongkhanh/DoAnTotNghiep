
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class ThesisRegistrationInput
    {
        public string ThesisId { get; set; }

        [Display(Name = "Tên nhóm đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string GroupName { get; set; }

        [Display(Name = "Mô tả nhóm đề tài")]
        public string GroupDescription { get; set; }

        [Required]
        public string RegisteredStudentId { get; set; }

        [Required]
        public string StudentIdList { get; set; }
    }
}
