using GraduateThesis.ApplicationCore.AppSettings;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.WebAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

#nullable disable

namespace GraduateThesis.ApplicationCore.AppController;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>

[HandleException]
public class WebControllerBase : Controller
{
    private void SetDefaultMsgViewData(DataResponseStatus dataResponseStatus)
    {
        if (dataResponseStatus == DataResponseStatus.Success && !string.IsNullOrEmpty(AppDefaultValue.SuccessMsg))
            ViewData["Message"] = AppDefaultValue.SuccessMsg;
        else if (dataResponseStatus == DataResponseStatus.InvalidData && !string.IsNullOrEmpty(AppDefaultValue.InvalidDataMsg))
            ViewData["Message"] = AppDefaultValue.InvalidDataMsg;
        else if (dataResponseStatus == DataResponseStatus.AlreadyExists && !string.IsNullOrEmpty(AppDefaultValue.AlreadyExistsMsg))
            ViewData["Message"] = AppDefaultValue.AlreadyExistsMsg;
        else if (dataResponseStatus == DataResponseStatus.NotFound && !string.IsNullOrEmpty(AppDefaultValue.NotFoundMsg))
            ViewData["Message"] = AppDefaultValue.NotFoundMsg;
        else if (dataResponseStatus == DataResponseStatus.HasConstraint && !string.IsNullOrEmpty(AppDefaultValue.HasConstraintMsg))
            ViewData["Message"] = AppDefaultValue.HasConstraintMsg;
        else if (dataResponseStatus == DataResponseStatus.Failed && !string.IsNullOrEmpty(AppDefaultValue.FailedMsg))
            ViewData["Message"] = AppDefaultValue.FailedMsg;
        else
            ViewData["Message"] = "The message content has not been set!";
    }

    private void SetDefaultMsgTempData(DataResponseStatus dataResponseStatus)
    {
        if (dataResponseStatus == DataResponseStatus.Success && !string.IsNullOrEmpty(AppDefaultValue.SuccessMsg))
            TempData["Message"] = AppDefaultValue.SuccessMsg;
        else if (dataResponseStatus == DataResponseStatus.InvalidData && !string.IsNullOrEmpty(AppDefaultValue.InvalidDataMsg))
            TempData["Message"] = AppDefaultValue.InvalidDataMsg;
        else if (dataResponseStatus == DataResponseStatus.AlreadyExists && !string.IsNullOrEmpty(AppDefaultValue.AlreadyExistsMsg))
            ViewData["Message"] = AppDefaultValue.AlreadyExistsMsg;
        else if (dataResponseStatus == DataResponseStatus.NotFound && !string.IsNullOrEmpty(AppDefaultValue.NotFoundMsg))
            TempData["Message"] = AppDefaultValue.NotFoundMsg;
        else if (dataResponseStatus == DataResponseStatus.HasConstraint && !string.IsNullOrEmpty(AppDefaultValue.HasConstraintMsg))
            TempData["Message"] = AppDefaultValue.HasConstraintMsg;
        else if (dataResponseStatus == DataResponseStatus.Failed && !string.IsNullOrEmpty(AppDefaultValue.FailedMsg))
            TempData["Message"] = AppDefaultValue.FailedMsg;
        else
            TempData["Message"] = "The message content has not been set!";
    }

    private void SetDefaultMsgViewData(AccountStatus accountStatus)
    {
        if (accountStatus == AccountStatus.Success && !string.IsNullOrEmpty(AppDefaultValue.AccAuthSuccessMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthSuccessMsg;
        else if (accountStatus == AccountStatus.InvalidData && !string.IsNullOrEmpty(AppDefaultValue.AccAuthInvalidDataMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthInvalidDataMsg;
        else if (accountStatus == AccountStatus.AlreadyExists && !string.IsNullOrEmpty(AppDefaultValue.AccAlreadyExistsMsg))
            ViewData["Message"] = AppDefaultValue.AccAlreadyExistsMsg;
        else if (accountStatus == AccountStatus.NotFound && !string.IsNullOrEmpty(AppDefaultValue.AccAuthNotFoundMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthNotFoundMsg;
        else if (accountStatus == AccountStatus.WrongPassword && !string.IsNullOrEmpty(AppDefaultValue.AccAuthWrongPwdMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthWrongPwdMsg;
        else if (accountStatus == AccountStatus.NotActivated && !string.IsNullOrEmpty(AppDefaultValue.AccAuthNotActivatedMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthNotActivatedMsg;
        else if (accountStatus == AccountStatus.Locked && !string.IsNullOrEmpty(AppDefaultValue.AccAuthLockedMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthLockedMsg;
        else if (accountStatus == AccountStatus.Failed && !string.IsNullOrEmpty(AppDefaultValue.AccAuthFailedMsg))
            ViewData["Message"] = AppDefaultValue.AccAuthFailedMsg;
        else
            ViewData["Message"] = "The message content has not been set!";
    }

    private void SetDefaultMsgTempData(AccountStatus accountStatus)
    {
        if (accountStatus == AccountStatus.Success && !string.IsNullOrEmpty(AppDefaultValue.AccAuthSuccessMsg))
            TempData["Message"] = AppDefaultValue.AccAuthSuccessMsg;
        else if (accountStatus == AccountStatus.InvalidData && !string.IsNullOrEmpty(AppDefaultValue.AccAuthInvalidDataMsg))
            TempData["Message"] = AppDefaultValue.AccAuthInvalidDataMsg;
        else if (accountStatus == AccountStatus.AlreadyExists && !string.IsNullOrEmpty(AppDefaultValue.AccAlreadyExistsMsg))
            ViewData["Message"] = AppDefaultValue.AccAlreadyExistsMsg;
        else if (accountStatus == AccountStatus.NotFound && !string.IsNullOrEmpty(AppDefaultValue.AccAuthNotFoundMsg))
            TempData["Message"] = AppDefaultValue.AccAuthNotFoundMsg;
        else if (accountStatus == AccountStatus.WrongPassword && !string.IsNullOrEmpty(AppDefaultValue.AccAuthWrongPwdMsg))
            TempData["Message"] = AppDefaultValue.AccAuthWrongPwdMsg;
        else if (accountStatus == AccountStatus.NotActivated && !string.IsNullOrEmpty(AppDefaultValue.AccAuthNotActivatedMsg))
            TempData["Message"] = AppDefaultValue.AccAuthNotActivatedMsg;
        else if (accountStatus == AccountStatus.Locked && !string.IsNullOrEmpty(AppDefaultValue.AccAuthLockedMsg))
            TempData["Message"] = AppDefaultValue.AccAuthLockedMsg;
        else if (accountStatus == AccountStatus.Failed && !string.IsNullOrEmpty(AppDefaultValue.AccAuthFailedMsg))
            TempData["Message"] = AppDefaultValue.AccAuthFailedMsg;
        else
            TempData["Message"] = "The message content has not been set!";
    }


    [NonAction]
    protected void AddViewData(DataResponse dataResponse)
    {
        ViewData["Status"] = dataResponse.Status.ToString();

        if (string.IsNullOrEmpty(dataResponse.Message))
            SetDefaultMsgViewData(dataResponse.Status);
        else
            ViewData["Message"] = dataResponse.Message;
    }

    [NonAction]
    protected void AddViewData(AccountAuthModel accountAuthModel)
    {
        ViewData["Status"] = accountAuthModel.Status.ToString();

        if (string.IsNullOrEmpty(accountAuthModel.Message))
            SetDefaultMsgViewData(accountAuthModel.Status);
        else
            ViewData["Message"] = accountAuthModel.Message;
    }

    [NonAction]
    protected void AddTempData(AccountAuthModel accountAuthModel)
    {
        TempData["Status"] = accountAuthModel.Status.ToString();

        if (string.IsNullOrEmpty(accountAuthModel.Message))
            SetDefaultMsgTempData(accountAuthModel.Status);
        else
            TempData["Message"] = accountAuthModel.Message;
    }

    [NonAction]
    protected void AddTempData(DataResponse dataResponse)
    {
        TempData["Status"] = dataResponse.Status.ToString();

        if (string.IsNullOrEmpty(dataResponse.Message))
            SetDefaultMsgTempData(dataResponse.Status);
        else
            TempData["Message"] = dataResponse.Message;
    }


    [NonAction]
    protected void AddViewData(DataResponseStatus dataResponseStatus)
    {
        AddViewData(new DataResponse { Status = dataResponseStatus });
    }

    [NonAction]
    protected void AddTempData(DataResponseStatus dataResponseStatus)
    {
        AddTempData(new DataResponse { Status = dataResponseStatus });
    }

    [NonAction]
    protected void AddViewData(AccountStatus accountStatus)
    {
        AddViewData(new AccountAuthModel { Status = accountStatus });
    }

    [NonAction]
    protected void AddTempData(AccountStatus accountStatus)
    {
        AddTempData(new AccountAuthModel { Status = accountStatus });
    }

    [NonAction]
    protected void AddViewData(DataResponseStatus dataResponseStatus, string message)
    {
        AddViewData(new DataResponse { Status = dataResponseStatus, Message = message });
    }

    [NonAction]
    protected void AddTempData(DataResponseStatus dataResponseStatus, string message)
    {
        AddTempData(new DataResponse { Status = dataResponseStatus, Message = message });
    }

    [NonAction]
    protected void AddViewData(AccountStatus accountStatus, string message)
    {
        AddViewData(new AccountAuthModel { Status = accountStatus, Message = message });
    }

    [NonAction]
    protected void AddTempData(AccountStatus accountStatus, string message)
    {
        AddTempData(new AccountAuthModel { Status = accountStatus, Message = message });
    }
}


/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TSubRepository"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class WebControllerBase<TSubRepository, TInput, TOutput, T_ID> : WebControllerBase
    where TSubRepository : class
    where TInput : class
    where TOutput : class
{
    private readonly IAsyncSubRepository<TInput, TOutput, T_ID> _asyncSubRepository;

    public WebControllerBase(TSubRepository subRepository)
    {
        _asyncSubRepository = (subRepository as IAsyncSubRepository<TInput, TOutput, T_ID>);
    }

    [NonAction]
    protected async Task<IActionResult> IndexResult(int page, int pageSize, string orderBy, string orderOptions, string keyword)
    {
        Pagination<TOutput> paginationInput = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC,
            SearchKeyword = keyword
        };

        Pagination<TOutput> pagination = await _asyncSubRepository.GetPaginationAsync(paginationInput);
        StaticPagedList<TOutput> pagedList = new StaticPagedList<TOutput>(pagination.Items, page, pageSize, pagination.TotalItemCount);

        Dictionary<string, string> orderByProperties = SetOrderByProperties();
        if(orderByProperties != null)
            ViewData["OrderByProperties"] = orderByProperties;

        Dictionary<string, string> searchByProperties = SetSearchByProperties();
        if (searchByProperties != null)
            ViewData["SearchByProperties"] = searchByProperties;

        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [NonAction]
    protected async Task<IActionResult> GetTrashResult(int count)
    {
        return View(await _asyncSubRepository.GetTrashAsync(count));
    }

    [NonAction]
    protected async Task<IActionResult> GetDetailsResult(T_ID id)
    {
        TOutput output = await _asyncSubRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        return View(output);
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult()
    {
        await LoadSelectListAsync();
        return View();
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult(TInput input)
    {
        await LoadSelectListAsync();
        if (ModelState.IsValid)
        {
            DataResponse<TOutput> dataResponse = await _asyncSubRepository.CreateAsync(input);
            AddViewData(dataResponse);

            return View(input);
        }

        AddViewData(DataResponseStatus.InvalidData);
        return View(input);
    }

    [NonAction]
    protected async Task<IActionResult> EditResult(T_ID id)
    {
        TOutput output = await _asyncSubRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        await LoadSelectListAsync();

        return View(output);
    }

    [NonAction]
    protected async Task<IActionResult> EditResult(T_ID id, TInput input)
    {
        await LoadSelectListAsync();
        if (ModelState.IsValid)
        {
            DataResponse<TOutput> dataResponse = await _asyncSubRepository.UpdateAsync(id, input);
            AddViewData(dataResponse);
            return View(input);
        }

        AddViewData(DataResponseStatus.InvalidData);
        return View(input);
    }

    [NonAction]
    protected async Task<IActionResult> BatchDeleteResult([Required] T_ID id)
    {
        DataResponse dataResponse = await _asyncSubRepository.BatchDeleteAsync(id);
        AddTempData(dataResponse);

        return RedirectToAction("Index");
    }

    [NonAction]
    protected async Task<IActionResult> ForceDeleteResult([Required] T_ID id)
    {
        DataResponse dataResponse = await _asyncSubRepository.ForceDeleteAsync(id);
        AddTempData(dataResponse);

        return RedirectToAction("GetTrash");
    }

    [NonAction]
    protected async Task<IActionResult> RestoreResult([Required] T_ID id)
    {
        DataResponse dataResponse = await _asyncSubRepository.RestoreAsync(id);
        AddTempData(dataResponse);

        return RedirectToAction("GetTrash");
    }

    [NonAction]
    protected async Task<IActionResult> ExportResult()
    {
        return await Task.Run(() =>
        {
            return View();
        });
    }

    [NonAction]
    protected async Task<IActionResult> ExportResult(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        return File(await _asyncSubRepository.ExportAsync(recordFilter, exportMetadata), ContentTypeConsts.XLSX);
    }

    [NonAction]
    protected async Task<IActionResult> ImportResult()
    {
        return await Task.Run(() =>
        {
            return View();
        });
    }

    [NonAction]
    protected async Task<IActionResult> ImportResult(IFormFile formFile, ImportMetadata importMetadata)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(DataResponseStatus.InvalidData);
            return View();
        }

        MemoryStream memoryStream = null;
        try
        {
            memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            DataResponse dataResponse = await _asyncSubRepository.ImportAsync(memoryStream, importMetadata);
            AddViewData(dataResponse);

            return View();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    protected virtual Task LoadSelectListAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Dictionary<string, string> SetOrderByProperties()
    {
        return null;
    }

    protected virtual Dictionary<string, string> SetSearchByProperties()
    {
        return null;
    }

    //public abstract Task<IActionResult> Index(Pagination<TOutput> pagination);
    public abstract Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "");
    public abstract Task<IActionResult> GetTrash(int count = 50);
    public abstract Task<IActionResult> Details([Required] T_ID id);
    public abstract Task<IActionResult> Create();
    public abstract Task<IActionResult> Create(TInput input);
    public abstract Task<IActionResult> Edit([Required] T_ID id);
    public abstract Task<IActionResult> Edit([Required] T_ID id, TInput input);
    public abstract Task<IActionResult> BatchDelete([Required] T_ID id);
    public abstract Task<IActionResult> Restore([Required] T_ID id);
    public abstract Task<IActionResult> ForceDelete([Required] T_ID id);
    public abstract Task<IActionResult> Export();
    public abstract Task<IActionResult> Export(ExportMetadata exportMetadata);
    public abstract Task<IActionResult> Import();
    public abstract Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata);
}
