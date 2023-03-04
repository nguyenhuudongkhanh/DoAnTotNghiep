using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class AsyncSubRepository<TEntity, TInput, TOutput, T_ID>
    : SubRepositoryBase<TEntity, TInput, TOutput, T_ID>, IAsyncSubRepository<TInput, TOutput, T_ID>
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    public AsyncSubRepository(DbContext dbContext, DbSet<TEntity> dbSet) 
        : base(dbContext, dbSet)
    {

    }

    protected virtual async Task<DataResponse<TOutput>> ValidateOnCreateAsync(TInput input)
    {
        await Task.CompletedTask;
        return new DataResponse<TOutput> { Status = DataResponseStatus.Success };
    }

    protected virtual async Task<DataResponse<TOutput>> ValidateOnUpdateAsync(TInput input)
    {
        await Task.CompletedTask;
        return new DataResponse<TOutput> { Status = DataResponseStatus.Success };
    }

    public virtual async Task<DataResponse> BatchDeleteAsync(T_ID id)
    {
        return await _genericRepository.BatchDeleteAsync(id);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _genericRepository.CountAsync();
    }

    public virtual async Task<DataResponse<TOutput>> CreateAsync(TInput input)
    {
        return await _genericRepository.CreateAsync(input, SetMapperToCreate, SetOutputMapper, input => ValidateOnCreateAsync(input));
    }

    public virtual async Task<byte[]> ExportAsync(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        return await _genericRepository.ExportAsync(recordFilter, exportMetadata, ListSelector);
    }

    public virtual async Task<DataResponse> ForceDeleteAsync(T_ID id)
    {
        return await _genericRepository.ForceDeleteAsync(id);
    }

    public virtual async Task<TOutput> GetAsync(T_ID id)
    {
        return await _genericRepository.GetAsync("Id", id, SingleSelector);
    }

    public virtual async Task<List<TOutput>> GetListAsync(int count)
    {
        return await _genericRepository.GetListAsync(count, ListSelector);
    }

    public virtual async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        Pagination<TOutput> pagination = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = orderOptions,
            SearchKeyword = keyword
        };

        return await _genericRepository.GetPaginationAsync(pagination, PaginationSelector);
    }

    public virtual async Task<Pagination<TOutput>> GetPaginationAsync(Pagination<TOutput> pagination)
    {
        return await _genericRepository.GetPaginationAsync(pagination, PaginationSelector);
    }

    public virtual async Task<List<TOutput>> GetTrashAsync(int count)
    {
        return await _genericRepository.GetTrashAsync(count, ListSelector);
    }

    public virtual async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<TEntity>
        {
            SimpleImportSpreadsheet = SimpleImportSelector
        });
    }

    public virtual async Task<DataResponse> RestoreAsync(T_ID id)
    {
        return await _genericRepository.RestoreAsync(id);
    }

    public virtual async Task<DataResponse<TOutput>> UpdateAsync(T_ID id, TInput input)
    {
        return await _genericRepository.UpdateAsync(id, input, SetMapperToUpdate, SetOutputMapper, input => ValidateOnUpdateAsync(input));
    }
}
