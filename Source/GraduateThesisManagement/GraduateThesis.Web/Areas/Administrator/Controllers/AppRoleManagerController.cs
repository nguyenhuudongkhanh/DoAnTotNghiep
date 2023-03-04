using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

#nullable disable

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/app-role-manager")]
public class AppRoleManagerController : WebControllerBase<IAppRoleRepository, AppRoleInput, AppRoleOutput, string>
{
    private IAppRoleRepository _appRoleRepository;
    private IAppPageRepository _appPageRepository;

    public AppRoleManagerController(IRepository repository) 
        : base(repository.AppRolesRepository)
    {
        _appRoleRepository = repository.AppRolesRepository;
        _appPageRepository = repository.AppPageRepository;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách quyền")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [NonAction]
    public override Task<IActionResult> Details([Required] string id)
    {
        throw new NotImplementedException();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết quyền")]
    public async Task<IActionResult> Details([Required] string id, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(id);
        if (appRole == null)
            return RedirectToAction("Index");

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<AppPageOutput> pagination = await _appPageRepository
            .GetPgnHasRoleIdAsync(id, page, pageSize, orderBy, orderOpts, keyword);

        StaticPagedList<AppPageOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "PageName", "Tên" }, { "ControllerName", "Tên bộ điều khiển" },
            { "ActionName", "Tên hành động" }, { "Area", "Khu vực" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View(appRole);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới quyền")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới quyền")]
    public override async Task<IActionResult> Create(AppRoleInput input)
    {
        return await CreateResult(input);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa quyền")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa quyền")]
    public override async Task<IActionResult> Edit([Required] string id, AppRoleInput input)
    {
        return await EditResult(id, input);
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [NonAction]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import([FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("add-page/{roleId}")]
    [HttpGet]
    [PageName(Name = "Thêm chức năng cho nhóm quyền")]
    public async Task<IActionResult> AddPage(string roleId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<AppPageOutput> pagination = await _appPageRepository
            .GetPgnHasNtRoleIdAsync(roleId, page, pageSize, orderBy, orderOpts, keyword);

        StaticPagedList<AppPageOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "PageName", "Tên" }, { "ControllerName", "Tên bộ điều khiển" },
            { "ActionName", "Tên hành động" }, { "Area", "Khu vực" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("add-page/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> AddPage(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.AddPageAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("AddPage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("AddPage", new { roleId = input.RoleId });
    }

    [Route("add-pages/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> AddPages(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.AddPagesAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("AddPage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("AddPage", new { roleId = input.RoleId });
    }

    [Route("remove-page/{roleId}")]
    [HttpGet]
    [PageName(Name = "Xóa chức năng ra khỏi nhóm quyền")]
    public async Task<IActionResult> RemovePage(string roleId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<AppPageOutput> pagination = await _appPageRepository
            .GetPgnHasRoleIdAsync(roleId, page, pageSize, orderBy, orderOpts, keyword);

        StaticPagedList<AppPageOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "PageName", "Tên" }, { "ControllerName", "Tên bộ điều khiển" },
            { "ActionName", "Tên hành động" }, { "Area", "Khu vực" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("remove-page/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> RemovePage(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.RemovePageAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("RemovePage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("RemovePage", new { roleId = input.RoleId });
    }

    [Route("remove-pages/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> RemovePages(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.RemovePagesAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("RemovePage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("RemovePage", new { roleId = input.RoleId });
    }

    [NonAction]
    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }
}
