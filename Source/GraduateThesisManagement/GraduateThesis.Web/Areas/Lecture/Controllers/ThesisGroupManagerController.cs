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
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/thesis-group-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class ThesisGroupManagerController : WebControllerBase<IThesisGroupRepository, ThesisGroupInput, ThesisGroupOutput, string>
{
    private readonly IThesisGroupRepository _thesisGroupRepository;
    private readonly IThesisRepository _thesisRepository;

    public ThesisGroupManagerController(IRepository repository)
        :base(repository.ThesisGroupRepository)
    {
        _thesisGroupRepository = repository.ThesisGroupRepository;
        _thesisRepository = repository.ThesisRepository;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    protected override Dictionary<string, string> SetSearchByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }
        };
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [NonAction]
    public override Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Create(ThesisGroupInput input)
    {
        throw new NotImplementedException();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết nhóm đề tài")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa nhóm đề tài")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa nhóm đề tài")]
    public override Task<IActionResult> Edit([Required] string id, ThesisGroupInput input)
    {
        return EditResult(id, input);
    }

    [Route("export")]
    public override async Task<IActionResult> Export()
    {
        return await ExportResult();
    }

    [Route("export")]
    [HttpPost]
    public override async Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        byte[] bytes = new byte[10];
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"student_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [NonAction]
    public override Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách nhóm sinh viên làm khóa luận")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisGroupOutput> pagination = await _thesisGroupRepository.GetPaginationAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisGroupOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("update-points/{thesisId}/{groupId}")]
    [HttpGet]
    [PageName(Name = "Cập nhật điểm cho nhóm thực hiện đề tài")]
    public async Task<IActionResult> UpdatePoints(string thesisId, string groupId)
    {
        List<StudentGroupDtInput> studentGroupDts = (await _thesisGroupRepository
            .GetStndtGrpDtsAsync(groupId)).Select(s => { return (s as StudentGroupDtInput); }).ToList();

        ViewData["Thesis"] = await _thesisRepository.GetAsync(thesisId);

        return View(new GroupPointInput { ThesisId = thesisId, GroupId = groupId, StudentPoints = studentGroupDts });
    }

    [Route("update-points/{thesisId}/{groupId}")]
    [HttpPost]
    public async Task<IActionResult> UpdatePoints(GroupPointInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("UpdatePoints", new { thesisId = input.ThesisId, groupId = input.GroupId });
        }

        DataResponse dataResponse = await _thesisGroupRepository.UpdatePointsAsync(input);
        if(dataResponse.Status == DataResponseStatus.Success)
        {
            return RedirectToAction("Details", "ThesisManager", new { id = input.ThesisId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("UpdatePoints", new { thesisId = input.ThesisId, groupId = input.GroupId });
    }
}
