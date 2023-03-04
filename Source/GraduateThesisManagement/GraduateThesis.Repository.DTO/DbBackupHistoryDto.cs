using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class DbBackupHistoryOutput
{
    [Display(Name = "Máy chủ")]
    public string Server { get; set; }

    [Display(Name = "Tên CSDL")]
    public string DatabaseName { get; set; }

    [Display(Name = "TG bắt đầu")]
    public DateTime StartDate { get; set; }

    [Display(Name = "TG kết thúc")]
    public DateTime FinishDate { get; set; }

    [Display(Name = "Ngày hết hạn")]
    public DateTime? ExpirationDate { get; set; }

    [Display(Name = "Loại sao lưu")]
    public string BackupType { get; set; }

    [Display(Name = "Kích thước")]
    public decimal BackupSize { get; set; }

    public string LogicalDeviceName { get; set; }
    public string PhysicalDeviceName { get; set; }

    public int MediaSetId { get; set; }

    public string BackupSetName { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }
}