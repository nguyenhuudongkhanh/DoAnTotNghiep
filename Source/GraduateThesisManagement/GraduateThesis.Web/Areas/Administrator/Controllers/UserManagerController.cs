using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

#nullable disable

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/user-manager")]
public class UserManagerController : WebControllerBase
{
    private readonly IFacultyStaffRepository _facultyStaffRepository;
    private readonly IAppRoleRepository _appRoleRepository;

    public UserManagerController(IRepository repository)
    {
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _appRoleRepository = repository.AppRolesRepository;
    }

    [Route("faculty-staff-list")]
    [HttpGet]
    [PageName(Name = "Danh sách nhân viên khoa")]
    public async Task<IActionResult> GetFacultyStaffs(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = null)
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository.GetPaginationAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["SearchByProperties"] = new Dictionary<string, string> {
            { "All", "Tất cả" }, { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }
        };

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("grant-role/{userId}")]
    [HttpGet]
    [PageName(Name = "Gán và thu hồi quyền của người dùng")]
    public async Task<IActionResult> GrantRole(string userId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        FacultyStaffOutput facultyStaff = await _facultyStaffRepository.GetAsync(userId);
        if (facultyStaff == null)
            return NotFound();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<AppRoleOutput> pagination = await _appRoleRepository
            .GetPgnHasNtUserId(userId, page, pageSize, orderBy, orderOpts, keyword);

        StaticPagedList<AppRoleOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string> {
            { "Id", "Mã" }, { "Name", "Tên" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["Keyword"] = keyword;
        ViewData["FacultyStaff"] = facultyStaff;

        return View(new AppUserRoleInput { UserId = userId });
    }

    [Route("grant-role/{userId}")]
    [HttpPost]
    public async Task<IActionResult> GrantRole(AppUserRoleInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GrantRole", new { userId = input.UserId });
        }

        DataResponse dataResponse = await _facultyStaffRepository.GrantRoleAsync(input);
        AddTempData(dataResponse);

        return RedirectToAction("GrantRole", new { userId = input.UserId });
    }

    [Route("revoke-role/{userId}")]
    [HttpPost]
    public async Task<IActionResult> RevokeRole(AppUserRoleInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GrantRole", new { userId = input.UserId });
        }

        DataResponse dataResponse = await _facultyStaffRepository.RevokeRoleAsync(input);
        AddTempData(dataResponse);

        return RedirectToAction("GrantRole", new { userId = input.UserId });
    }
}
