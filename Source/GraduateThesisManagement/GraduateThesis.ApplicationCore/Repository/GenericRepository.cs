using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>

public partial class GenericRepository<TEntity, TInput, TOutput>
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    private DbContext _context;
    private DbSet<TEntity> _dbSet;
    private Converter _converter;
    private ExpressionBuilder _expressionBuilder;
    private PaginationHelper<TEntity, TOutput> _paginationHelper;
    private ImportHelper<TEntity> _importHelper;
    private ExportHelper<TOutput> _exportHelper;
    private Expression<Func<TEntity, object>>[] _navigationPropertyPaths;

    public GenericRepository(DbContext context, DbSet<TEntity> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
        _converter = new Converter();
        _expressionBuilder = new ExpressionBuilder();
        _paginationHelper = new PaginationHelper<TEntity, TOutput>(dbSet, _expressionBuilder);
        _exportHelper = new ExportHelper<TOutput>();
        _importHelper = new ImportHelper<TEntity>(context, dbSet, _converter);
        _navigationPropertyPaths = null;
    }


    #region include (EF)

    public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
    {
        if (navigationPropertyPaths != null)
        {
            _navigationPropertyPaths = navigationPropertyPaths;
            _paginationHelper.IncludeMany(navigationPropertyPaths);
        }
    }

    #endregion

    #region get records methods

    public Pagination<TOutput> GetPagination(Pagination<TOutput> pagination, Expression<Func<TEntity, TOutput>> selector)
    {
        IQueryable<TEntity> queryable = _paginationHelper.GetQueryableForPagination(pagination.OrderBy, pagination.OrderOptions, pagination.SearchKeyword);
        pagination.TotalItemCount = queryable.Count();

        return _paginationHelper.GetPagination(queryable, pagination, selector);
    }

    public List<TOutput> GetList(int count, Expression<Func<TEntity, TOutput>> selector)
    {
        return _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
            .Where("x => x.IsDeleted == false").Select(selector).ToList();
    }

    public TOutput GetByPk(params object[] keyValues)
    {
        TEntity entity = _dbSet.Find(keyValues);
        if (entity == null)
            return default(TOutput)!;

        return _converter.To<TEntity, TOutput>(entity);
    }

    public TOutput Get(string propertyName, object value, Expression<Func<TEntity, TOutput>> selector)
    {
        return _dbSet.IncludeMultiple(_navigationPropertyPaths)
            .Where(_expressionBuilder.GetWhereExpString("x", propertyName, value))
            .Select(selector).SingleOrDefault();
    }

    #endregion

    
    #region create new records methods

    public DataResponse Create(TInput input, Action<TInput, TEntity> mapToEntity)
    {
        TEntity entity = Activator.CreateInstance<TEntity>();

        mapToEntity(input, entity);
        _dbSet.Add(entity);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse<TOutput> Create(TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput)
    {
        TEntity entity = Activator.CreateInstance<TEntity>();

        mapToEntity(input, entity);
        _dbSet.Add(entity);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        TOutput output = Activator.CreateInstance<TOutput>();
        mapToOutput(entity, output);

        return new DataResponse<TOutput> { 
            Status = DataResponseStatus.Success,
            Data = output
        };
    }

    public DataResponse Create(TInput input, Action<TInput, TEntity> mapToEntity, Func<TInput, DataResponse> validate)
    {
        DataResponse validationResponse = validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        return Create(input, mapToEntity);
    }

    public DataResponse<TOutput> Create(TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput, Func<TInput, DataResponse<TOutput>> validate)
    {
        DataResponse<TOutput> validationResponse = validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        return Create(input, mapToEntity, mapToOutput);
    }

    public DataResponse BulkInsert(IEnumerable<TInput> inputs, Func<TInput, TEntity> predicate)
    {
        _dbSet.BulkInsert(_converter.To<TInput, TEntity>(inputs, predicate));
        _context.BulkSaveChanges();

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region update records methods

    public DataResponse Update(object id, TInput input, Action<TInput, TEntity> mapToEntity)
    {
        TEntity entity_fromDb = _dbSet.Find(id);
        if (entity_fromDb == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        mapToEntity(input, entity_fromDb);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse<TOutput> Update(object id, TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput)
    {
        TEntity entity_fromDb = _dbSet.Find(id);
        if (entity_fromDb == null)
            return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

        mapToEntity(input, entity_fromDb);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        TOutput output = Activator.CreateInstance<TOutput>();
        mapToOutput(entity_fromDb, output);

        return new DataResponse<TOutput>
        {
            Status = DataResponseStatus.Success,
            Data = output
        };
    }

    public DataResponse Update(object id, TInput input, Action<TInput, TEntity> mapToEntity, Func<TInput, DataResponse> validate)
    {
        TEntity entity_fromDb = _dbSet.Find(id);
        if (entity_fromDb == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        DataResponse validationResponse = validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        mapToEntity(input, entity_fromDb);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse<TOutput> Update(object id, TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput, Func<TInput, DataResponse<TOutput>> validate)
    {
        TEntity entity_fromDb = _dbSet.Find(id);
        if (entity_fromDb == null)
            return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

        DataResponse<TOutput> validationResponse = validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        mapToEntity(input, entity_fromDb);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        TOutput output = Activator.CreateInstance<TOutput>();
        mapToOutput(entity_fromDb, output);

        return new DataResponse<TOutput>
        {
            Status = DataResponseStatus.Success,
            Data = output
        };
    }

    #endregion


    #region delete records methods

    public DataResponse BatchDelete(object id)
    {
        TEntity entity = _dbSet.Find(id);
        if (entity == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        Type entityType = entity.GetType();
        PropertyInfo isDeletedPropertyInfo = entityType.GetProperty("IsDeleted");
        if (isDeletedPropertyInfo == null)
            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Failed,
                Message = "Property named 'IsDeleted' not found"
            };

        isDeletedPropertyInfo.SetValue(entity, true);

        PropertyInfo deletedAtPropertyInfo = entityType.GetProperty("DeletedAt");
        if (deletedAtPropertyInfo != null)
            deletedAtPropertyInfo.SetValue(entity, DateTime.Now);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse ForceDelete(object id)
    {
        TEntity entity = _dbSet.Find(id);
        if (entity == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        Type entityType = entity.GetType();
        PropertyInfo isDeletedPropertyInfo = entityType.GetProperty("IsDeleted");
        if (isDeletedPropertyInfo != null)
        {
            bool isDeleted = (bool)isDeletedPropertyInfo.GetValue(entity)!;
            if (!isDeleted)
                return new DataResponse { Status = DataResponseStatus.Failed };
        }

        _dbSet.Remove(entity);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse ForceDelete(object id, Func<bool> predicate)
    {
        if (!predicate())
            return new DataResponse { Status = DataResponseStatus.HasConstraint };

        return ForceDelete(id);
    }

    public DataResponse ForceDeleteMany(object[] idList)
    {
        List<TEntity> entities = new List<TEntity>();
        foreach (object id in idList)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            entities.Add(entity);
        }

        _dbSet.RemoveRange(entities);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region restore records methods

    public DataResponse Restore(object id)
    {
        TEntity entity = _dbSet.Find(id);
        if (entity == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        Type entityType = entity.GetType();
        PropertyInfo isDeletedPropertyInfo = entityType.GetProperty("IsDeleted");
        if (isDeletedPropertyInfo == null)
            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Failed,
                Message = "Property named 'IsDeleted' not found"
            };

        isDeletedPropertyInfo.SetValue(entity, false);

        PropertyInfo deletedAtPropertyInfo = entityType.GetProperty("DeletedAt");
        if (deletedAtPropertyInfo != null)
            deletedAtPropertyInfo.SetValue(entity, null);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region get trash records methods

    public List<TOutput> GetTrash(int count, Expression<Func<TEntity, TOutput>> selector)
    {
        return _dbSet.Where("x => x.IsDeleted == true").Select(selector).ToList();
    }

    #endregion

    #region count records methods

    public int Count()
    {
        return _dbSet.Count();
    }

    public long LongCount()
    {
        return _dbSet.LongCount();
    }

    #endregion


    #region export to files

    public byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata, Expression<Func<TEntity, TOutput>> selector)
    {
        List<TOutput> outputs = _dbSet.Where(recordFilter.BuildExpString("p", null))
            .Take(exportMetadata.MaxRecordNumber).Select(selector).ToList();

        if (exportMetadata.TypeOptions == ExportTypeOptions.XLS || exportMetadata.TypeOptions == ExportTypeOptions.XLSX)
            return _exportHelper.ExportToSpreadsheet(outputs, exportMetadata.TypeOptions, exportMetadata.SheetName, exportMetadata.IncludeProperties);
        else
            throw new Exception("This file type is not supported");
    }

    #endregion


    #region import from files

    public DataResponse Import(Stream stream, ImportMetadata importMetadata, ImportSelector<TEntity> importSelector)
    {
        if(importMetadata.TypeOptions == ImportTypeOptions.XLS || importMetadata.TypeOptions == ImportTypeOptions.XLSX)
        {
            if (importSelector.SimpleImportSpreadsheet != null)
                return _importHelper.ImportFromSpreadsheet(stream, importMetadata.StartFromRow, importSelector.SimpleImportSpreadsheet);
            else if (importSelector.SimpleImportSpreadsheet != null)
                return _importHelper.ImportFromSpreadsheet(stream, importMetadata.StartFromRow, importMetadata.SheetName, importSelector.AdvancedImportSpreadsheet);
            else
                throw new Exception("No matching selector found!");
        }
        else
        {
            throw new Exception("This file type is not supported!");
        }
    }

    #endregion
}
