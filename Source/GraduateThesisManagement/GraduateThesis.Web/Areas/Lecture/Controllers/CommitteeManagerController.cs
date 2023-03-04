using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/committee-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class CommitteeManagerController : WebControllerBase<IThesisCommitteeRepository, ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
    private readonly IThesisCommitteeRepository _thesisCommitteeRepository;
    private readonly IFacultyStaffRepository _facultyStaffRepository;
    private readonly ICouncilRepository _councilRepository;
    private readonly IThesisRepository _thesisRepository;
    private readonly IAccountManager _accountManager;

    public CommitteeManagerController(IRepository repository, IAuthorizationManager authorizationManager)
        : base(repository.ThesisCommitteeRepository)
    {
        _thesisCommitteeRepository = repository.ThesisCommitteeRepository;
        _councilRepository = repository.CouncilRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _thesisRepository = repository.ThesisRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    protected override async Task LoadSelectListAsync()
    {
        List<CouncilOutput> councils = await _councilRepository.GetListAsync(50);
        ViewData["CouncilSelectList"] = new SelectList(councils, "Id", "Name");
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới tiểu ban")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới tiểu ban")]
    public override async Task<IActionResult> Create(ThesisCommitteeInput input)
    {
        return await CreateResult(input);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết tiểu ban")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }


    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa  tiểu ban")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa  tiểu ban")]
    public override async Task<IActionResult> Edit([Required] string id, ThesisCommitteeInput input)
    {
        return await EditResult(id, input);
    }

    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
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

    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Import([FromForm, Required] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các tiểu ban")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("add-member-view/{committeeId}")]
    [HttpGet]
    [PageName(Name = "Thêm thành viên vào tiểu ban")]
    public async Task<IActionResult> LoadAddMemberView(string committeeId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        ThesisCommitteeOutput thesisCommittee = await _thesisCommitteeRepository.GetAsync(committeeId);
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
         .GetPaginationAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);

        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
            {
                { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }, { "CreatedAt", "Ngày tạo" }
            };

        ViewData["SearchByProperties"] = new Dictionary<string, string>
            {
                { "All", "Tất cả" }, { "Id", "Mã" }, { "Surname", "Họ" }, { "Name", "Tên" }, { "Email", "Email" }
            };

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View(thesisCommittee);
    }

    [Route("add-member")]
    [HttpPost]
    public async Task<IActionResult> AddMember(CommitteeMemberInput input)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.AddMemberAsync(input);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAddMemberView", new { committeeId = input.CommitteeId });
    }

    [Route("delete-member")]
    [HttpPost]
    public async Task<IActionResult> DeleteMember([Required] string committeeId, [Required] string memberId)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.DeleteMemberAsync(committeeId, memberId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAddMemberView", new { committeeId = committeeId });
    }

    [Route("add-thesis-view/{committeeId}")]
    [HttpGet]
    [PageName(Name = "Thêm thành viên vào tiểu ban")]
    public async Task<IActionResult> LoadAddThesisView(string committeeId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        ThesisCommitteeOutput thesisCommittee = await _thesisCommitteeRepository.GetAsync(committeeId);
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository
         .GetPgnToAssignCmteAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);

        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["SearchByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" }
        };

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View(thesisCommittee);
    }

    [Route("add-thesis")]
    [HttpPost]
    public async Task<IActionResult> AddThesis([Required] string committeeId, [Required] string thesisId)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.AddThesisAsync(committeeId, thesisId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAddThesisView", new { committeeId = committeeId });
    }

    [Route("delete-thesis")]
    [HttpPost]
    public async Task<IActionResult> DeleteThesis([Required] string committeeId, [Required] string thesisId)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.DeleteThesisAsync(committeeId, thesisId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAddThesisView", new { committeeId = committeeId });
    }

    [Route("get-my-cmtes")]
    [HttpGet]
    [PageName(Name = "Danh sách tiểu ban của tôi")]
    public async Task<IActionResult> GetCmtesOfLecturer(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisCommitteeOutput> pagination = await _thesisCommitteeRepository.GetPaginationAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisCommitteeOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("get-my-cmte/{committeeId}")]
    [HttpGet]
    [PageName(Name = "Chi tiết tiểu ban")]
    public async Task<IActionResult> GetCmteOfLecturer(string committeeId)
    {
        return await GetDetailsResult(committeeId);
    }
}
