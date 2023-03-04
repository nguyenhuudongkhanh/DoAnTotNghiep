﻿using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/topic-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class TopicManagerController : WebControllerBase<ITopicRepository, TopicInput, TopicOutput, string>
{
    private readonly ITopicRepository _topicRepository;

    public TopicManagerController(IRepository repository) : base(repository.TopicRepository)
    {
        _topicRepository = repository.TopicRepository;
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
    [PageName(Name = "Danh sách các chủ đề khóa luận")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết chủ đề khóa luận")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới chủ đề khóa luận")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới chủ đề khóa luận")]
    public override async Task<IActionResult> Create(TopicInput input)
    {
        return await CreateResult(input);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id, TopicInput input)
    {
        return await EditResult(id, input);
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

    [NonAction]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
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
