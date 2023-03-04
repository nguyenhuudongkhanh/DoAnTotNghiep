using GraduateThesis.ApplicationCore.ApiAttributes;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GraduateThesis.ApplicationCore.AppController;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TSubRepository"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

[ApiHandleException]
public abstract class ApiControllerBase<TSubRepository, TInput, TOutput, T_ID> : ControllerBase
    where TSubRepository : class
    where TInput : class
    where TOutput : class
{
    private readonly IAsyncSubRepository<TInput, TOutput, T_ID> _asyncSubRepository;
    private readonly IFileManager _fileManager;

    public ApiControllerBase(TSubRepository subRepository, IFileManager fileManager)
    {
        _asyncSubRepository = (subRepository as IAsyncSubRepository<TInput, TOutput, T_ID>);
        _fileManager = fileManager;
    }

    [NonAction]
    protected IActionResult GetApiResult(DataResponse dataResponse)
    {
        if (dataResponse.Status == DataResponseStatus.Success)
            return Ok(dataResponse);
        else if (dataResponse.Status == DataResponseStatus.NotFound)
            return NotFound(dataResponse);
        else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
            return StatusCode(409, dataResponse);
        else
            return BadRequest(dataResponse);
    }

    [NonAction]
    protected async Task<IActionResult> GetPaginationResult(Pagination<TOutput> pagination)
    {
        return Ok(await _asyncSubRepository.GetPaginationAsync(pagination));
    }

    [NonAction]
    protected async Task<IActionResult> GetListResult(int count)
    {
        return Ok(await _asyncSubRepository.GetListAsync(count));
    }

    [NonAction]
    protected virtual async Task<IActionResult> GetDetailsResult(T_ID id)
    {
        TOutput output = await _asyncSubRepository.GetAsync(id);
        if (output == null)
            return NotFound();

        return Ok(output);
    }

    [NonAction]
    protected virtual async Task<IActionResult> CreateResult(TInput input)
    {
        DataResponse<TOutput> dataResponse = await _asyncSubRepository.CreateAsync(input);
        return GetApiResult(dataResponse);
    }

    [NonAction]
    protected virtual async Task<IActionResult> UpdateResult(T_ID id, [FromBody] TInput input)
    {
        DataResponse<TOutput> dataResponse = await _asyncSubRepository.UpdateAsync(id, input);
        return GetApiResult(dataResponse);
    }

    [NonAction]
    protected virtual async Task<IActionResult> BatchDeleteResult(T_ID id)
    {
        DataResponse dataResponse = await _asyncSubRepository.BatchDeleteAsync(id);
        return GetApiResult(dataResponse);
    }

    [NonAction]
    protected virtual async Task<IActionResult> ForceDeleteResult(T_ID id)
    {
        DataResponse dataResponse = await _asyncSubRepository.ForceDeleteAsync(id);
        return GetApiResult(dataResponse);
    }

    [NonAction]
    protected async Task<IActionResult> ExportResult(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _asyncSubRepository.ExportAsync(null, exportMetadata);

        string fileExtension = _fileManager.GetExtension(exportMetadata.TypeOptions);
        string fileName = $"{exportMetadata.FileName}_{DateTime.Now.ToString("ddMMyyyy_hhmmss")}.{fileExtension}";

        return File(bytes, _fileManager.GetContentType(exportMetadata.TypeOptions), fileName);
    }

    [NonAction]
    protected async Task<IActionResult> ImportResult(IFormFile formFile, ImportMetadata importMetadata)
    {
        MemoryStream memoryStream = null;
        try
        {
            DataResponse dataResponse = await _asyncSubRepository.ImportAsync(memoryStream, new ImportMetadata());
            return Ok(dataResponse);
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }


    [Route("pagination")]
    [HttpPost]
    public abstract Task<IActionResult> GetPagination(Pagination<TOutput> pagination);

    [Route("details/{id}")]
    [HttpGet]
    public abstract Task<IActionResult> GetDetails([Required] T_ID id);

    [Route("create")]
    [HttpPost]
    public abstract Task<IActionResult> Create(TInput input);

    [Route("update/{id}")]
    [HttpPost]
    public abstract Task<IActionResult> Update([Required] T_ID id, TInput input);

    [Route("batch-delete/{id}")]
    [HttpDelete]
    public abstract Task<IActionResult> BatchDelete([Required] T_ID id);

    [Route("force-delete/{id}")]
    [HttpDelete]
    public abstract Task<IActionResult> ForceDelete([Required] T_ID id);

    [Route("export")]
    [HttpPost]
    public abstract Task<IActionResult> Export(ExportMetadata exportMetadata);

    [Route("import")]
    [HttpPost]
    public abstract Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata);
}
