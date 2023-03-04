using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.AppSettings;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("system-manager")]
public class SystemManagerController : WebControllerBase
{
    private ISystemRepository _systemRepository;

    public SystemManagerController(IRepository repository)
    {
        _systemRepository = repository.SystemRepository;
    }

    [Route("exec-query")]
    [HttpGet]
    public IActionResult ExecQuery()
    {
        return View();
    }

    [Route("exec-query")]
    [HttpPost]
    public IActionResult ExecQuery(ExecQueryInput execQueryInput)
    {
        if (ModelState.IsValid)
        {
            //int affected = _repository.ExecuteNonQuery($"{execQueryInput.Query}");
            return View();
        }

        return View(execQueryInput);
    }

    [Route("command")]
    [HttpGet]
    public IActionResult Command()
    {
        return View();
    }

    [Route("command")]
    [HttpPost]
    public IActionResult Command(IFormCollection form)
    {
        return View();
    }

    [Route("backup-database-view")]
    [HttpGet]
    [PageName(Name = "Sao lưu cơ sở dữ liệu")]
    public IActionResult LoadBackupDbView()
    {
        ViewData["DbName"] = AppDefaultValue.DbName;
        ViewData["DbBackupFilePath"] = AppDefaultValue.DbBackupFilePath;

        return View();
    }

    [Route("backup-database")]
    [HttpPost]
    public IActionResult BackupDatabase()
    {
        _systemRepository.BackupDatabase();
        AddTempData(DataResponseStatus.Success);

        return RedirectToAction("GetDbBackupHistory");
    }

    [Route("backup-database-history")]
    [HttpGet]
    [PageName(Name = "Lịch sử sao lưu cơ sở dữ liệu")]
    public IActionResult GetDbBackupHistory()
    {
        return View(_systemRepository.GetDbBackupHistory());
    }

    [Route("clear-database-backup-history")]
    [HttpPost]
    public IActionResult ClearDbBackupHistory()
    {
        _systemRepository.ClearDbBackupHistory();
        AddTempData(DataResponseStatus.Success);

        return RedirectToAction("GetDbBackupHistory");
    }

    [Route("restore-database/{mediaSetId}")]
    [HttpGet]
    [PageName(Name = "Xem và xác nhận phục hồi bản sao lưu")]
    public IActionResult RestoreDatabase(int mediaSetId)
    {
        DbBackupHistoryOutput dbBackupHistory = _systemRepository.GetBackupHistoryDt(mediaSetId);
        if (dbBackupHistory == null)
            return RedirectToAction("GetDbBackupHistory");

        return View(dbBackupHistory);
    }

    [Route("restore-database/{mediaSetId}")]
    [HttpPost]
    public IActionResult RestoreDbComfirmed(int mediaSetId)
    {
        _systemRepository.RestoreDatabase(mediaSetId);
        AddTempData(DataResponseStatus.Success);

        return RedirectToAction("RestoreDatabase", new { mediaSetId = mediaSetId });
    }
}
