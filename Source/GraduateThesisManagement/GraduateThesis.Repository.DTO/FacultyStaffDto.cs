using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class FacultyStaffInput
{
    [Display(Name = " Mã giảng viên khoa")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Id { get; set; }

    [Display(Name = "Mã khoa")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string FacultyId { get; set; }

    [Display(Name = "Họ giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Surname { get; set; }

    [Display(Name = "Tên giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Giới tính")]
    [RegularExpression("Nam|Nữ", ErrorMessage = "{0} chỉ có 'Nam' hoặc 'Nữ'")]
    public string Gender { get; set; }

    [Display(Name = "Số điện thoại")]
    [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
    [Phone(ErrorMessage = "{0} không hợp lệ")]
    public string Phone { get; set; }

    [Display(Name = "Địa chỉ")]
    public string Address { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Email { get; set; }

    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "Ảnh đại diện")]
    public string Avatar { get; set; }

    [Display(Name = "Chức vụ")]
    public string Position { get; set; }

    [Display(Name = "Bằng cấp")]
    public string Degree { get; set; }

    [Display(Name = "Ghi chú")]
    public string Notes { get; set; }
}

public class FacultyStaffOutput : FacultyStaffInput
{
    [Display(Name = "Tên giảng viên")]
    public string FullName { get { return $"{Surname.Trim(' ')} {Name.Trim(' ')}"; } }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public FacultyOutput Faculty { get; set; }
    public List<AppRoleOutput> Roles { get; set; }
}

public class FacultyStaffExport
{
    public int Index { get; set; }
    public string Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    [ExcelFormat("dd/MM/yyyy")]
    public DateTime? Birthday { get; set; }
}

