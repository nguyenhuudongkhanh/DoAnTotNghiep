using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Student.Controllers;

[Area("Student")]
[Route("student/thesis")]
[IsStudent]
[AccountInfo(typeof(StudentOutput))]
public class StudentThesisController : WebControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly IThesisRepository _thesisRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IThesisGroupRepository _thesisGroupRepository;
    private readonly IThesisRevisionRepository _thesisRevisionRepository;

    public StudentThesisController(IRepository repository, IAuthorizationManager authorizationManager)
    {
        _thesisRepository = repository.ThesisRepository;
        _studentRepository = repository.StudentRepository;
        _thesisGroupRepository = repository.ThesisGroupRepository;
        _thesisRevisionRepository = repository.ThesisRevisionRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài khóa luận")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string searchBy = "All", string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        DataResponse<string> dataResponse = await _studentRepository.CheckRegThesisAsync(userId);
        if (dataResponse.Status != DataResponseStatus.Success)
        {
            if (dataResponse.Data == "NotFound")
                return NotFound();

            if (dataResponse.Data == "Registered" || dataResponse.Data == "Completed" || dataResponse.Data == "Invited")
                return RedirectToAction("GetThesisGroups");
        }

        OrderOptions orderOpts = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC;
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfPubldThesesAsync(page, pageSize, orderBy, orderOpts, searchBy, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["OrderByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" }, { "CreatedAt", "Ngày tạo" }
        };

        ViewData["SearchByProperties"] = new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "LectureName", "Tên GV" }, { "Year", "Năm học" },
        };

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["SearchBy"] = searchBy;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Xem chi tiết đề tài")]
    public async Task<IActionResult> Details([Required] string id)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(id);
        if (thesis == null)
            return RedirectToAction("GetList");

        return View(thesis);
    }

    [Route("search-students")]
    [HttpGet]
    public async Task<IActionResult> SearchStudents(string keyword)
    {
        return Json(await _studentRepository.SearchForThesisRegAsync(keyword));
    }

    [Route("get-student-by-id")]
    [HttpGet]
    public async Task<IActionResult> GetStudentById(string studentId)
    {
        return Json(await _studentRepository.GetForThesisRegAsync(studentId));
    }

    [Route("can-add-member")]
    [HttpPost]
    public async Task<IActionResult> CanAddMember(string thesisId, int currentStudentNumber)
    {
        return Json(await _thesisRepository.CanAddMember(thesisId, currentStudentNumber));
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Đăng ký đề tài")]
    public IActionResult Register([Required] string studentId, [Required] string thesisId)
    {
        ViewData["StudentId"] = studentId;
        ViewData["ThesisId"] = thesisId;
        return View(new ThesisRegistrationInput());
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Đăng ký đề tài")]
    public async Task<IActionResult> Register(ThesisRegistrationInput thesisRegistrationInput)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _thesisRepository.RegisterThesisAsync(thesisRegistrationInput);
            AddTempData(dataResponse);
            if (dataResponse.Status == DataResponseStatus.Success)
                return RedirectToAction("GetThesisGroups");

            AddViewData(dataResponse);
            return View(thesisRegistrationInput);
        }

        AddTempData(DataResponseStatus.InvalidData);
        return View(thesisRegistrationInput);
    }

    [Route("join-to-group")]
    [HttpPost]
    public async Task<IActionResult> JoinToGroupAsync([Required] string groupId)
    {
        _accountManager.SetHttpContext(HttpContext);
        DataResponse dataResponse = await _thesisGroupRepository.JoinToGroupAsync(_accountManager.GetUserId(), groupId);
        AddTempData(dataResponse);

        return RedirectToAction("GetThesisGroups");
    }

    [Route("deny-from-group")]
    [HttpPost]
    public async Task<IActionResult> DenyFromGroupAsync([Required] string groupId)
    {
        _accountManager.SetHttpContext(HttpContext);
        DataResponse dataResponse = await _thesisGroupRepository.DenyFromGroupAsync(_accountManager.GetUserId(), groupId);
        AddTempData(dataResponse);

        return RedirectToAction("GetThesisGroups");
    }

    [Route("thesis-groups")]
    [HttpGet]
    [PageName(Name = "Danh sách nhóm đồ án của bạn")]
    public async Task<IActionResult> GetThesisGroups()
    {
        _accountManager.SetHttpContext(HttpContext);
        List<ThesisGroupDtOutput> thesisGroups = await _thesisGroupRepository
            .GetGroupsByStdntIdAsync(_accountManager.GetUserId());

        return View(thesisGroups);
    }

    [Route("my-thesis/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Đề tài khóa luận của bạn")]
    public async Task<IActionResult> GetThesis([Required] string thesisId)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if (thesis == null)
            return NotFound();

        ViewData["SupvResult"] = await _thesisRepository.GetSupvResultAsync(thesis.Id);
        ViewData["CtrArgResult"] = await _thesisRepository.GetCtrArgResultAsync(thesis.Id);

        if (thesis.ThesisGroupId != null)
            ViewData["StudentGroupDts"] = await _thesisGroupRepository.GetStndtGrpDtsAsync(thesis.ThesisGroupId);

        return View(thesis);
    }

    [Route("get-revisions/{thesisId}/{groupId}")]
    [HttpGet]
    [PageName(Name = "Xem tiến độ đề tài")]
    public async Task<IActionResult> GetRevisions([Required] string thesisId, [Required] string groupId)
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        DataResponse<string> dataResponse = await _thesisRepository.CheckHasThesisAsync(thesisId, userId);
        if (dataResponse.Status == DataResponseStatus.NotFound)
            return NotFound();

        ViewData["InProgress"] = await _thesisRepository.CheckIsInprAsync(thesisId);
        ViewData["IsLeader"] = await _thesisGroupRepository.CheckIsLeaderAsync(userId, groupId);
        ViewData["Revisions"] = await _thesisRevisionRepository.GetRevsByThesisIdAsync(thesisId);

        return View(new ThesisRevisionInput { ThesisId = thesisId, GroupId = groupId });
    }

    public async Task<IActionResult> AddRevision(ThesisRevisionInput input)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetRevisions", new { thesisId = input.ThesisId, groupId = input.GroupId });
        }

        DataResponse dataResponse = await _thesisRevisionRepository.CreateAsync(input);
        AddTempData(dataResponse);

        return RedirectToAction("GetRevisions", new { thesisId = input.ThesisId, groupId = input.GroupId });
    }

    [Route("submit-thesis/{thesisId}/{groupId}")]
    [HttpGet]
    [PageName(Name = "Nộp đề tài khóa luận")]
    public async Task<IActionResult> SubmitThesis([Required] string thesisId, string groupId)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if (thesis == null)
            return NotFound();

        ViewData["Thesis"] = thesis;

        return View(new ThesisSubmissionInput { ThesisId = thesisId, GroupId = groupId });
    }

    [Route("submit-thesis/{thesisId}/{groupId}")]
    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    [PageName(Name = "Nộp đề tài")]
    public async Task<IActionResult> SubmitThesis(ThesisSubmissionInput input)
    {
        DataResponse dataResponse = await _thesisRepository.SubmitThesisAsync(input);
        if(dataResponse.Status == DataResponseStatus.Success)
        {
            AddTempData(dataResponse);
            return RedirectToAction("GetThesisGroups");
        }

        ThesisOutput thesis = await _thesisRepository.GetAsync(input.ThesisId);
        ViewData["Thesis"] = thesis;
        AddTempData(dataResponse);

        return View(input);
    }
}
