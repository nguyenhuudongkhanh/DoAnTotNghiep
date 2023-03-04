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
{

    #region get records methods

    public async Task<Pagination<TOutput>> GetPaginationAsync(Pagination<TOutput> pagination, Expression<Func<TEntity, TOutput>> selector)
    {
        IQueryable<TEntity> queryable = _paginationHelper.GetQueryableForPagination(pagination.OrderBy, pagination.OrderOptions, pagination.SearchKeyword);
        pagination.TotalItemCount = await queryable.CountAsync();

        return await _paginationHelper.GetPaginationAsync(queryable, pagination, selector);
    }

    public async Task<List<TOutput>> GetListAsync(int count, Expression<Func<TEntity, TOutput>> selector)
    {
        return await _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
            .Where("x => x.IsDeleted == false").Select(selector).ToListAsync();
    }

    public async Task<TOutput> GetByPkAsync(params object[] keyValues)
    {
        TEntity entity = await _dbSet.FindAsync(keyValues);
        if (entity == null)
            return default(TOutput);

        return _converter.To<TEntity, TOutput>(entity);
    }

    public async Task<TOutput> GetAsync(string propertyName, object value, Expression<Func<TEntity, TOutput>> selector)
    {
        return await _dbSet.IncludeMultiple(_navigationPropertyPaths)
            .Where(_expressionBuilder.GetWhereExpString("x", propertyName, value))
            .Select(selector).SingleOrDefaultAsync();
    }

    #endregion

    #region create new records methods

    public async Task<DataResponse> CreateAsync(TInput input, Action<TInput, TEntity> mapToEntity)
    {
        TEntity entity = Activator.CreateInstance<TEntity>();

        mapToEntity(input, entity);
        _dbSet.Add(entity);

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<DataResponse<TOutput>> CreateAsync(TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput)
    {
        TEntity entity = Activator.CreateInstance<TEntity>();

        mapToEntity(input, entity);
        _dbSet.Add(entity);

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        TOutput output = Activator.CreateInstance<TOutput>();
        mapToOutput(entity, output);

        return new DataResponse<TOutput>
        {
            Status = DataResponseStatus.Success,
            Data = output
        };
    }

    public async Task<DataResponse> CreateAsync(TInput input, Action<TInput, TEntity> mapToEntity, Func<TInput, Task<DataResponse>> validate)
    {
        DataResponse validationResponse = await validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        return await CreateAsync(input, mapToEntity);
    }

    public async Task<DataResponse<TOutput>> CreateAsync(TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput, Func<TInput, Task<DataResponse<TOutput>>> validate)
    {
        DataResponse<TOutput> validationResponse = await validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        return await CreateAsync(input, mapToEntity, mapToOutput);
    }

    public async Task<DataResponse> BulkInsertAsync(IEnumerable<TInput> inputs, Func<TInput, TEntity> predicate)
    {
        await _dbSet.BulkInsertAsync(_converter.To<TInput, TEntity>(inputs, predicate));
        await _context.BulkSaveChangesAsync();

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion

    #region update records methods

    public async Task<DataResponse> UpdateAsync(object id, TInput input, Action<TInput, TEntity> mapToEntity)
    {
        TEntity entity_fromDb = await _dbSet.FindAsync(id);
        if (entity_fromDb == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        mapToEntity(input, entity_fromDb);

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<DataResponse<TOutput>> UpdateAsync(object id, TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput)
    {
        TEntity entity_fromDb = await _dbSet.FindAsync(id);
        if (entity_fromDb == null)
            return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

        mapToEntity(input, entity_fromDb);

        int affected = await _context.SaveChangesAsync();
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

    public async Task<DataResponse> UpdateAsync(object id, TInput input, Action<TInput, TEntity> mapToEntity, Func<TInput, Task<DataResponse>> validate)
    {
        TEntity entity_fromDb = await _dbSet.FindAsync(id);
        if (entity_fromDb == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        DataResponse validationResponse = await validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        mapToEntity(input, entity_fromDb);

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<DataResponse<TOutput>> UpdateAsync(object id, TInput input, Action<TInput, TEntity> mapToEntity, Action<TEntity, TOutput> mapToOutput, Func<TInput, Task<DataResponse<TOutput>>> validate)
    {
        TEntity entity_fromDb = await _dbSet.FindAsync(id);
        if (entity_fromDb == null)
            return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

        DataResponse<TOutput> validationResponse = await validate(input);
        if (validationResponse.Status != DataResponseStatus.Success)
            return validationResponse;

        mapToEntity(input, entity_fromDb);

        int affected = await _context.SaveChangesAsync();
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

    public async Task<DataResponse> BatchDeleteAsync(object id)
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

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<DataResponse> ForceDeleteAsync(object id)
    {
        TEntity entity = await _dbSet.FindAsync(id);
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

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<DataResponse> ForceDeleteAsync(object id, Func<Task<bool>> predicate)
    {
        if (!await predicate())
            return new DataResponse { Status = DataResponseStatus.HasConstraint };

        return await ForceDeleteAsync(id);
    }

    public async Task<DataResponse> ForceDeleteManyAsync(object[] idList)
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

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region restore records methods

    public async Task<DataResponse> RestoreAsync(object id)
    {
        TEntity entity = await _dbSet.FindAsync(id);
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

        int affected = await _context.SaveChangesAsync();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region

    public async Task<List<TOutput>> GetTrashAsync(int count, Expression<Func<TEntity, TOutput>> selector)
    {
        return await _dbSet.Where("x => x.IsDeleted == true").Select(selector).ToListAsync();
    }

    #endregion


    #region count records methods

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<long> LongCountAsync()
    {
        return await _dbSet.LongCountAsync();
    }

    #endregion

    #region export to files

    public async Task<byte[]> ExportAsync(RecordFilter recordFilter, ExportMetadata exportMetadata, Expression<Func<TEntity, TOutput>> selector)
    {
        //recordFilter.BuildExpString("p", null)
        List<TOutput> outputs = await _dbSet.Where("x => x.IsDeleted == false")
            .Take(exportMetadata.MaxRecordNumber).Select(selector).ToListAsync();

        if (exportMetadata.TypeOptions == ExportTypeOptions.XLS || exportMetadata.TypeOptions == ExportTypeOptions.XLSX)
            return _exportHelper.ExportToSpreadsheet(outputs, exportMetadata.TypeOptions, exportMetadata.SheetName, exportMetadata.IncludeProperties);
        else
            throw new Exception("This file type is not supported");
    }

    #endregion


    #region import from files

    public async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata, ImportSelector<TEntity> importSelector)
    {
        if (importMetadata.TypeOptions == ImportTypeOptions.XLS || importMetadata.TypeOptions == ImportTypeOptions.XLSX)
        {
            if (importSelector.SimpleImportSpreadsheet != null)
                return await _importHelper.ImportFromSpreadsheetAsync(stream, importMetadata.StartFromRow, importSelector.SimpleImportSpreadsheet);
            else if (importSelector.AdvancedImportSpreadsheet != null)
                return await _importHelper.ImportFromSpreadsheetAsync(stream, importMetadata.StartFromRow, importMetadata.SheetName, importSelector.AdvancedImportSpreadsheet);
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
