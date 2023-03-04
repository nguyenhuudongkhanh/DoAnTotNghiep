using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.AppController;
using X.PagedList;
using GraduateThesis.WebExtensions;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.File;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using GraduateThesis.Repository.BLL.Consts;

#nullable disable

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/thesis-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class ThesisManagerController : WebControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
{
    private readonly IAccountManager _accountManager;
    private readonly ITopicRepository _topicRepository;
    private readonly ITrainingFormRepository _trainingFormRepository;
    private readonly ITrainingLevelRepository _trainingLevelRepository;
    private readonly IThesisRepository _thesisRepository;
    private readonly IThesisRevisionRepository _thesisRevisionRepository;
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IFacultyStaffRepository _facultyStaffRepository;
    private readonly IThesisGroupRepository _thesisGroupRepository;

    public ThesisManagerController(IRepository repository, IAuthorizationManager authorizationManager)
        : base(repository.ThesisRepository)
    {
        _thesisRepository = repository.ThesisRepository;
        _thesisRevisionRepository = repository.ThesisRevisionRepository;
        _trainingFormRepository = repository.TrainingFormRepository;
        _trainingLevelRepository = repository.TrainingLevelRepository;
        _topicRepository = repository.TopicRepository;
        _specializationRepository = repository.SpecializationRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _thesisGroupRepository = repository.ThesisGroupRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    protected override Dictionary<string, string> SetSearchByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" }
        };
    }

    protected override async Task LoadSelectListAsync()
    {
        List<TopicOutput> topics = await _topicRepository.GetListAsync(50);
        ViewData["TopicSelectList"] = new SelectList(topics, "Id", "Name");

        List<TrainingLevelOutput> trainingLevels = await _trainingLevelRepository.GetListAsync(50);
        ViewData["TrainingLevelSelectList"] = new SelectList(trainingLevels, "Id", "Name");

        List<TrainingFormOutput> trainingForms = await _trainingFormRepository.GetListAsync(50);
        ViewData["TrainingFormSelectList"] = new SelectList(trainingForms, "Id", "Name");

        List<SpecializationOutput> specializations = await _specializationRepository.GetListAsync(50);
        ViewData["SpecializationsSelectList"] = new SelectList(specializations, "Id", "Name");
    }

    [NonAction]
    public override Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các đề tài khóa luận")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

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
    [PageName(Name = "Chi tiết đề tài khóa luận")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(id);
        if (thesis == null)
            return NotFound();

        ViewData["SupvResult"] = await _thesisRepository.GetSupvResultAsync(thesis.Id);
        ViewData["CtrArgResult"] = await _thesisRepository.GetCtrArgResultAsync(thesis.Id);

        if (thesis.ThesisGroupId != null)
            ViewData["StudentGroupDts"] = await _thesisGroupRepository.GetStndtGrpDtsAsync(thesis.ThesisGroupId);
 
        return View(thesis);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới đề tài khóa luận")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới đề tài khóa luận")]
    public override async Task<IActionResult> Create(ThesisInput thesisInput)
    {
        return await CreateResult(thesisInput);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id, ThesisInput thesisInput)
    {
        ViewData["Lecturer"] = await _facultyStaffRepository.GetAsync(thesisInput.LectureId);
        return await EditResult(id, thesisInput);
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
        byte[] bytes = await _thesisRepository.ExportAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"thesis_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-pending-theses")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách đề tài đang chờ duyệt")]
    public async Task<IActionResult> ExportPendingTheses()
    {
        return await ExportResult();
    }

    [Route("export-pending-theses")]
    [HttpPost]
    public async Task<IActionResult> ExportPendingTheses(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _thesisRepository.ExportPndgThesesAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"pending-thesis_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-rejected-theses")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách đề tài đã bị từ chối duyệt")]
    public async Task<IActionResult> ExportRejectedTheses()
    {
        return await ExportResult();
    }

    [Route("export-rejected-theses")]
    [HttpPost]
    public async Task<IActionResult> ExportRejectedTheses(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _thesisRepository.ExportRejdThesesAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"rejected-thesis_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-approved-theses")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách đề tài đã được duyệt")]
    public async Task<IActionResult> ExportApprovedTheses()
    {
        return await ExportResult();
    }

    [Route("export-approved-theses")]
    [HttpPost]
    public async Task<IActionResult> ExportApprovedTheses(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _thesisRepository.ExportAppdThesesAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"approved-thesis_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-published-theses")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách đề tài đã được công bố")]
    public async Task<IActionResult> ExportPublishedTheses()
    {
        return await ExportResult();
    }

    [Route("export-published-theses")]
    [HttpPost]
    public async Task<IActionResult> ExportPublishedTheses(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _thesisRepository.ExportPubldThesesAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"published-thesis_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-theses-in-progress")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách đề tài đang được thực hiện")]
    public async Task<IActionResult> ExportThesesInpr()
    {
        return await ExportResult();
    }

    [Route("export-theses-in-progress")]
    [HttpPost]
    public async Task<IActionResult> ExportThesesInpr(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _thesisRepository.ExportThesesInprAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"thesis-in-progress_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
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

    [Route("approve-thesis/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Xét duyệt đề tài")]
    public async Task<IActionResult> ApproveThesis([Required] string thesisId)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if (thesis == null)
            return NotFound();

        if (thesis.StatusId >= ThesisStatusConsts.Approved)
            return RedirectToAction("GetPendingList");

        ViewData["Thesis"] = thesis;

        return View(new ThesisApprovalInput { ThesisId = thesis.Id });
    }

    [Route("approve-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Xét duyệt đề tài")]
    public async Task<IActionResult> ApproveThesis(ThesisApprovalInput approvalInput)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("ApproveThesis", new { thesisId = approvalInput.ThesisId });
        }

        DataResponse dataResponse = await _thesisRepository.ApproveThesisAsync(approvalInput);
        AddTempData(dataResponse);

        return RedirectToAction("GetPendingList");
    }

    [Route("reject-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Từ chối xét duyệt đề tài")]
    public async Task<IActionResult> RejectThesis(ThesisApprovalInput approvalInput)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("ApproveThesis", new { thesisId = approvalInput.ThesisId });
        }

        DataResponse dataResponse = await _thesisRepository.RejectThesisAsync(approvalInput);
        AddTempData(dataResponse);

        return RedirectToAction("GetPendingList");
    }

    [Route("publish-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Công bố đề tài")]
    public async Task<IActionResult> PublishThesis([Required] string thesisId)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetApprovedList");
        }

        DataResponse dataResponse = await _thesisRepository.PublishThesisAsync(thesisId);
        AddTempData(dataResponse);

        return RedirectToAction("GetApprovedList");
    }

    [Route("publish-theses")]
    [HttpPost]
    [PageName(Name = "Công bố đề tài")]
    public async Task<IActionResult> PublishTheses([Required] string thesisIds)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetApprovedList");
        }

        DataResponse dataResponse = await _thesisRepository
            .PublishThesesAsync(thesisIds.Split(new char[] { ';' }));
        AddTempData(dataResponse);

        return RedirectToAction("GetApprovedList");
    }

    [Route("stop-publishing-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Ngừng công bố đề tài")]
    public async Task<IActionResult> StopPublishingThesis([Required] string thesisId)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetPublishedList");
        }

        DataResponse dataResponse = await _thesisRepository.StopPubgThesisAsync(thesisId);
        AddTempData(dataResponse);

        return RedirectToAction("GetPublishedList");
    }

    [Route("stop-publishing-theses")]
    [HttpPost]
    [PageName(Name = "Công bố đề tài")]
    public async Task<IActionResult> StopPublishingTheses([Required] string thesisIds)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetPublishedList");
        }

        DataResponse dataResponse = await _thesisRepository
            .StopPubgThesesAsync(thesisIds.Split(new char[] { ';' }));
        AddTempData(dataResponse);

        return RedirectToAction("GetPublishedList");
    }

    [Route("pending-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đang chờ xét duyệt")]
    public async Task<IActionResult> GetPendingList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfPndgApvlThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("approved-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã được duyệt")]
    public async Task<IActionResult> GetApprovedList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfAppdThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("rejected-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài bị từ chối xét duyệt")]
    public async Task<IActionResult> GetRejectedList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfRejdThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("published-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã công bố")]
    public async Task<IActionResult> GetPublishedList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfPubldThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("inprogress-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã công bố")]
    public async Task<IActionResult> GetInProgressList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfThesesInprAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("submitted-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài mà sinh viên đã nộp")]
    public async Task<IActionResult> GetSubmittedList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfSubmdThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("finished-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã hoàn thành")]
    public async Task<IActionResult> GetFinishedList(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfFinishedThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("my-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài của tôi")]
    public async Task<IActionResult> GetThesesOfLecturer(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPaginationAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("my-pending-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đang chờ duyệt của tôi")]
    public async Task<IActionResult> GetPndgThesesOfLecturer(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfPndgApvlThesesAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("my-rejected-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài bị từ chối xét duyệt của tôi")]
    public async Task<IActionResult> GetRejdThesesOfLecturer(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfRejdThesesAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("my-approved-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài bị từ chối xét duyệt của tôi")]
    public async Task<IActionResult> GetAppdThesesOfLecturer(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfAppdThesesAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("revisions/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Tiến độ của đề tài")]
    public async Task<IActionResult> GetRevisions(string thesisId)
    {
        return View(await _thesisRevisionRepository.GetRevsByThesisIdAsync(thesisId));
    }

    [Route("review-revision/{revisionId}")]
    [HttpGet]
    [PageName(Name = "Nhận xét về bản thay đổi")]
    public async Task<IActionResult> ReviewRevision(string revisionId)
    {
        ThesisRevisionOutput thesisRevision = await _thesisRevisionRepository.GetAsync(revisionId);
        if (thesisRevision == null)
            return NotFound();

        if (thesisRevision.Reviewed)
            return RedirectToAction("GetRevisions", new { thesisId = thesisRevision.Thesis.Id });

        ViewData["ThesisRevision"] = thesisRevision;

        return View(new ThesisRevReviewInput { RevisionId = thesisRevision.Id });
    }

    [Route("review-revision/{revisionId}")]
    [HttpPost]
    [PageName(Name = "Nhận xét về bản thay đổi")]
    public async Task<IActionResult> ReviewRevision(ThesisRevReviewInput thesisRevReview)
    {
        ThesisRevisionOutput thesisRevision = await _thesisRevisionRepository.GetAsync(thesisRevReview.RevisionId);
        if (!ModelState.IsValid)
        {
            AddViewData(DataResponseStatus.InvalidData);
            ViewData["ThesisRevision"] = thesisRevision;

            return View(new ThesisRevReviewInput { RevisionId = thesisRevision.Id });
        }

        DataResponse dataResponse = await _thesisRevisionRepository.ReviewRevisionAsync(thesisRevReview);
        AddTempData(dataResponse);

        return RedirectToAction("GetRevisions", new { thesisId = thesisRevision.Thesis.Id });
    }


    [Route("get-theses-to-assign-supervisor")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài cần phân công GVHD")]
    public async Task<IActionResult> GetThesesToAssignSupv(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnToAssignSupvAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("get-assigned-supervisor-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã phân công GVHD")]
    public async Task<IActionResult> GetAssignedSupvTheses(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfAssignedSupvAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("assign-supervisor/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Phân công GVHD của đề tài khóa luận")]
    public async Task<IActionResult> LoadAssignSupvView(string thesisId, int page = 1, int pageSize = 5, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if(thesis.ThesisSupervisor == null)
        {
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
        }

        return View(thesis);
    }

    [Route("assign-default-supervisor")]
    [HttpPost]
    public async Task<IActionResult> AssignDefautSupv(string thesisId)
    {
        DataResponse dataResponse = await _thesisRepository.AssignSupervisorAsync(thesisId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAssignSupvView", new { thesisId = thesisId });
    }

    [Route("assign-supervisor")]
    [HttpPost]
    public async Task<IActionResult> AssignSupervisor(string thesisId, string lecturerId)
    {
        DataResponse dataResponse = await _thesisRepository.AssignSupervisorAsync(thesisId, lecturerId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAssignSupvView", new { thesisId = thesisId});
    }

    [Route("assign-supervisors")]
    [HttpPost]
    public async Task<IActionResult> AssignSupervisors(string thesisIds)
    {
        DataResponse dataResponse = await _thesisRepository.AssignSupervisorsAsync(thesisIds.Split(new char[] { ';' }));
        AddTempData(dataResponse);

        return RedirectToAction("GetThesesToAssignSupv");
    }

    [Route("remove-assign-supervisor")]
    [HttpPost]
    public async Task<IActionResult> RemoveAssignSupv(string thesisId, string lecturerId)
    {
        DataResponse dataResponse = await _thesisRepository.RemoveAssignSupvAsync(thesisId, lecturerId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAssignSupvView", new { thesisId = thesisId });
    }

    [Route("get-theses-to-assign-critical-lecturer")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài cần phân công GVPB")]
    public async Task<IActionResult> GetThesesToAssignCLect(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnToAssignCLectAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("get-assigned-cleturer-theses")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã phân công GVPB")]
    public async Task<IActionResult> GetAssignedCLectTheses(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfAssignedCLectAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("assign-critical-lecturer/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Phân công GVPB đề tài khóa luận")]
    public async Task<IActionResult> LoadAssignCLectView(string thesisId, string roleId, int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if(thesis.CriticalLecturer == null)
        {
            OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
            Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
                .GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, searchBy, keyword);

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
        }

        return View(thesis);
    }

    [Route("assign-critical-lecturer")]
    [HttpPost]
    [PageName(Name = "Phân công giảng viên phản biện")]
    public async Task<IActionResult> AssignCLect(string thesisId, string lecturerId)
    {
        DataResponse dataResponse = await _thesisRepository.AssignCLectAsync(thesisId, lecturerId);
        AddTempData(dataResponse);

        return RedirectToAction("LoadAssignCLectView", new { thesisId = thesisId });
    }

    [Route("update-supv-result/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Phiếu đánh giá của GVHD")]
    public async Task<IActionResult> UpdateSupvResult(string thesisId)
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        ViewData["thesis"] = thesis;

        return View(new SupvResultInput { ThesisId = thesisId, LecturerId = userId });
    }

    [Route("update-supv-result/{thesisId}")]
    [HttpPost]
    public async Task<IActionResult> UpdateSupvResult(SupvResultInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("UpdateSupvResult", new { thesisId = input.ThesisId });
        }

        DataResponse dataResponse = await _thesisRepository.UpdateSupvResultAsync(input);
        if (dataResponse.Status == DataResponseStatus.Success)
            return RedirectToAction("Details", new { id = input.ThesisId });

        AddTempData(dataResponse);
        return RedirectToAction("UpdateSupvResult", new { thesisId = input.ThesisId });
    }

    [Route("update-ctrarg-result/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Phiếu đánh giá của GVPB")]
    public async Task<IActionResult> UpdateCtrArgResult(string thesisId)
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        ViewData["Thesis"] = thesis;

        return View(new CtrArgResultInput { ThesisId = thesisId, LecturerId = userId });
    }

    [Route("update-ctrarg-result/{thesisId}")]
    [HttpPost]
    public async Task<IActionResult> UpdateCtrArgResult(CtrArgResultInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("UpdateCtrArgResult", new { thesisId = input.ThesisId });
        }

        DataResponse dataResponse = await _thesisRepository.UpdateCtrArgResultAsync(input);
        if (dataResponse.Status == DataResponseStatus.Success)
            return RedirectToAction("Details", new { id = input.ThesisId });

        AddTempData(dataResponse);
        return RedirectToAction("UpdateCtrArgResult", new { thesisId = input.ThesisId });
    }

    [Route("get-theses-to-supervise")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài cần hướng dẫn của tôi")]
    public async Task<IActionResult> GetThesesToSupv(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnToSupvAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("get-theses-to-criticize")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài cần phản biện của tôi")]
    public async Task<IActionResult> GetThesesToCriticize(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnToCriticizeAsync(userId, page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = SetOrderByProperties();
        ViewData["SearchByProperties"] = SetSearchByProperties();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("export-to-supervise")]
    [HttpGet]
    [PageName(Name = "Xuất danh sách các đề tài cần HD của tôi")]
    public async Task<IActionResult> ExportThesesToSupv()
    {
        return await ExportResult();
    }

    [Route("export-to-supervise")]
    [HttpPost]
    public async Task<IActionResult> ExportThesesToSupv(ExportMetadata exportMetadata)
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        byte[] bytes = await _thesisRepository.ExportThesesToSupv(userId);
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"thesis-in-progress_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }
}