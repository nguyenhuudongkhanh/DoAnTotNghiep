using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisInput
{
    [Display(Name = "Mã đề tài")]
    public string Id { get; set; }

    [Display(Name = "Tên đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Các mô tả, yêu cầu")]
    public string Description { get; set; }

    [Display(Name = "Số sinh viên tối đa")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(1, 3, ErrorMessage = "{0} có giá từ 1 đến 3")]
    public int MaxStudentNumber { get; set; }

    public string File { get; set; }

    [Display(Name = "Số tín chỉ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(4, 8, ErrorMessage = "{0} có giá trị từ 4 đến 8")]
    public int Credits { get; set; }

    [Display(Name = "Năm học")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Year { get; set; }

    [Display(Name = "Ghi chú")]
    public string Notes { get; set; }

    [Display(Name = "Chủ đề")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string TopicId { get; set; }

    [Display(Name = "Hình thức đào tạo")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string TrainingFormId { get; set; }

    [Display(Name = "Bậc đào tạo")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string TrainingLevelId { get; set; }

    [Display(Name = "Chuyên ngành")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string SpecializationId { get; set; }

    [Display(Name = "Từ ngày")]
    public DateTime? DateFrom { get; set; }

    [Display(Name = "Đến ngày")]
    public DateTime? DateTo { get; set; }

    [Display(Name = "Giảng viên ra đề")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string LectureId { get; set; }

    [Display(Name = "Học kỳ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(1, 3, ErrorMessage = "{0} có giá trì từ 1 đến 3")]
    public int Semester { get; set; }

    public string ThesisGroupId { get; set; }

    public int StatusId { get; set; }
}

public class ThesisOutput : ThesisInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public TopicOutput Topic { get; set; }

    public ThesisGroupOutput ThesisGroup { get; set; }

    public TrainingLevelOutput TrainingLevel { get; set; }

    public TrainingFormOutput TrainingForm { get; set; }

    public FacultyStaffOutput Lecturer { get; set; }

    public FacultyStaffOutput ThesisSupervisor { get; set; }

    public FacultyStaffOutput CriticalLecturer { get; set; }

    public SpecializationOutput Specialization { get; set; }

    public ThesisStatusOutput ThesisStatus { get; set; }
}

public class PublishedThesisOutput
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxStudentNumber { get; set; }
    public FacultyStaffOutput Lecturer { get; set; }
    public FacultyStaffOutput CriticalLecturer { get; set; }
    public FacultyStaffOutput ThesisSupervisor { get; set; }
    public List<StudentOutput> Students { get; set; }
}

public class ThesisExport
{
    public int Index { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxStudentNumber { get; set; }
    public int Credits { get; set; }
    public string Year { get; set; }
    public int Semester { get; set; }
    public string SpecializationName { get; set; }
    public string TopicName { get; set; }
    public string LecturerName { get; set; }
    public string Notes { get; set; }
    public string SupervisorName { get; set; }
    public string CLectName { get; set; }
    public string Students { get; set; }
}