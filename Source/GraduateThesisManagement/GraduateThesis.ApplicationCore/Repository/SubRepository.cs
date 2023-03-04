using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class SubRepository<TEntity, TInput, TOutput, T_ID>
    : SubRepositoryBase<TEntity, TInput, TOutput, T_ID>, ISubRepository<TInput, TOutput, T_ID>
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    public SubRepository(DbContext dbContext, DbSet<TEntity> dbSet)
        : base(dbContext, dbSet)
    {

    }

    protected virtual DataResponse<TOutput> ValidateOnCreate(TInput input)
    {
        return new DataResponse<TOutput> { Status = DataResponseStatus.Success };
    }

    protected virtual DataResponse<TOutput> ValidateOnUpdate(TInput input)
    {
        return new DataResponse<TOutput> { Status = DataResponseStatus.Success };
    }

    public virtual DataResponse BatchDelete(T_ID id)
    {
        return _genericRepository.BatchDelete(id);
    }

    public virtual int Count()
    {
        return _genericRepository.Count();
    }

    public virtual DataResponse<TOutput> Create(TInput input)
    {
        return _genericRepository.Create(input, SetMapperToCreate, SetOutputMapper, input => ValidateOnCreate(input));
    }

    public virtual byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        return _genericRepository.Export(recordFilter, exportMetadata, ListSelector);
    }

    public virtual DataResponse ForceDelete(T_ID id)
    {
        return _genericRepository.ForceDelete(id);
    }

    public virtual TOutput Get(T_ID id)
    {
        return _genericRepository.Get("Id", id, SingleSelector);
    }

    public virtual List<TOutput> GetList(int count)
    {
        return _genericRepository.GetList(count, ListSelector);
    }

    public virtual Pagination<TOutput> GetPagination(Pagination<TOutput> pagination)
    {
        return _genericRepository.GetPagination(pagination, PaginationSelector);
    }

    public virtual DataResponse Import(Stream stream, ImportMetadata importMetadata)
    {
        return _genericRepository.Import(stream, importMetadata, new ImportSelector<TEntity>
        {
            SimpleImportSpreadsheet = SimpleImportSelector
        });
    }

    public virtual DataResponse<TOutput> Update(T_ID id, TInput input)
    {
        return _genericRepository.Update(id, input, SetMapperToUpdate, SetOutputMapper, input => ValidateOnUpdate(input));
    }

    public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        Pagination<TOutput> pagination = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = orderOptions,
            SearchKeyword = keyword
        };

        return _genericRepository.GetPagination(pagination, PaginationSelector);
    }

    public List<TOutput> GetTrash(int count)
    {
        return _genericRepository.GetTrash(count, ListSelector);
    }

    public DataResponse Restore(T_ID id)
    {
        return _genericRepository.Restore(id);
    }
}
