using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using X.PagedList;

#nullable disable

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/facutly-staff-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class FacultyStaffManagerController : WebControllerBase<IFacultyStaffRepository, FacultyStaffInput, FacultyStaffOutput, string>
{
    private readonly IFacultyRepository _facultyRepository;
    private readonly IFacultyStaffRepository _facultyStaffRepository;

    public FacultyStaffManagerController(IRepository repository)
        : base(repository.FacultyStaffRepository)
    {
        _facultyRepository = repository.FacultyRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    protected override Dictionary<string, string> SetSearchByProperties()
    {
        return new Dictionary<string, string> {
            { "All", "Tất cả" }, { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }
        };
    }

    protected override async Task LoadSelectListAsync()
    {
        List<FacultyOutput> faculties = await _facultyRepository.GetListAsync(50);
        ViewData["FacultySelectList"] = new SelectList(faculties, "Id", "Name");
    }

    [NonAction]
    public override Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách giảng viên")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = null)
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository.GetPaginationAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết giảng viên")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới giảng viên")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới giảng viên")]
    public override async Task<IActionResult> Create(FacultyStaffInput facultyStaffInput)
    {
        return await CreateResult(facultyStaffInput);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa giảng viên")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa giảng viên")]
    public override async Task<IActionResult> Edit([Required] string id, FacultyStaffInput facultyStaffInput)
    {
        return await EditResult(id, facultyStaffInput);
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("export")]
    [HttpGet]
    [PageName(Name = "Xuất dữ liệu ra khỏi hệ thống")]
    public override async Task<IActionResult> Export()
    {
        return await ExportResult();
    }

    [Route("export")]
    [HttpPost]
    public override async Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _facultyStaffRepository.ExportAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"faculty-staff_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        if (string.IsNullOrEmpty(importMetadata.SheetName))
            importMetadata.SheetName = "Default";

        importMetadata.StartFromRow = 2;
        return await ImportResult(formFile, importMetadata);
    }

    [Route("import")]
    [HttpGet]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import()
    {
        return await ImportResult();
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }
}
