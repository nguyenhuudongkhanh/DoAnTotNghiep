using ExcelDataReader;
using GraduateThesis.ApplicationCore.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class SubRepositoryBase<TEntity, TInput, TOutput, T_ID> 
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    protected GenericRepository<TEntity, TInput, TOutput> _genericRepository;

    public Expression<Func<TEntity, TOutput>> PaginationSelector { get; set; }
    public Expression<Func<TEntity, TOutput>> ListSelector { get; set; }
    public Expression<Func<TEntity, TOutput>> SingleSelector { get; set; }
    public Func<IExcelDataReader, TEntity> SimpleImportSelector { get; set; }
    public Func<DataRow, TEntity> AdvancedImportSelector { get; set; }

    public SubRepositoryBase(DbContext dbContext, DbSet<TEntity> dbSet)
    {
        _genericRepository = new GenericRepository<TEntity, TInput, TOutput>(dbContext, dbSet);

        ConfigureIncludes();
        ConfigureSelectors();
    }

    public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
    {
        _genericRepository.IncludeMany(navigationPropertyPaths);
    }

    protected abstract void ConfigureIncludes();
    protected abstract void ConfigureSelectors();
    protected abstract void SetOutputMapper(TEntity entity, TOutput output);
    protected abstract void SetMapperToUpdate(TInput input, TEntity entity);
    protected abstract void SetMapperToCreate(TInput input, TEntity entity);
}
