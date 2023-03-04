using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

internal class PaginationHelper<TEntity, TOutput>
    where TEntity : class
    where TOutput : class
{
    public DbSet<TEntity> _dbSet;
    public ExpressionBuilder _expressionBuilder;
    public Expression<Func<TEntity, object>>[] _navigationPropertyPaths;

    public PaginationHelper(DbSet<TEntity> dbSet, ExpressionBuilder expressionBuilder)
    {
        _dbSet = dbSet;
        _expressionBuilder = expressionBuilder;
        _navigationPropertyPaths = null!;
    }

    public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
    {
        _navigationPropertyPaths = navigationPropertyPaths;
    }

    public IOrderedQueryable<TEntity> GetOrderedQueryable(IQueryable<TEntity> queryable, string orderBy, OrderOptions orderOptions)
    {
        if (string.IsNullOrEmpty(orderBy))
            return queryable.OrderBy(_expressionBuilder.GetOrderByExpString<TEntity>("p", "CreatedAt"));

        if (orderOptions == OrderOptions.ASC)
            return queryable.OrderBy(_expressionBuilder.GetOrderByExpString<TEntity>("p", orderBy));

        return queryable.OrderByDescending(_expressionBuilder.GetOrderByExpression<TEntity>("p", orderBy));
    }

    public IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string keyword)
    {
        IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
        if (!string.IsNullOrEmpty(keyword))
            queryable = queryable.Where(_expressionBuilder.GetWhereExpString<TEntity>("p", keyword));
        else
            queryable = queryable.Where(_expressionBuilder.GetWhereExpString("p"));
        queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

        return queryable;
    }

    public IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string filterBy, object filterValue)
    {
        IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
        if (!string.IsNullOrEmpty(filterBy))
            queryable = queryable.Where(_expressionBuilder.GetWhereExpString<TEntity>("p", filterBy, filterValue));
        else
            queryable = queryable.Where(_expressionBuilder.GetWhereExpString("p"));
        queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

        return queryable;
    }

    public IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string[] conditions, string keyword)
    {
        if (conditions == null)
            throw new Exception("'conditions' must not be null!");
        if (conditions.Length == 0)
            throw new Exception("'conditions' must not be empty!");

        IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
        if (!string.IsNullOrEmpty(keyword))
        {
            queryable = queryable.Where(_expressionBuilder.GetWhereExpString<TEntity>("p", conditions, keyword));
        }
        queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

        return queryable;
    }

    //public IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, RecordFilter recordFilter)
    //{
    //    IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
    //    if (recordFilter != null)
    //        queryable = queryable.Where(_expressionBuilder.GetWhereExpString<TEntity>("p", recordFilter));
    //    else
    //        queryable = queryable.Where(_expressionBuilder.GetWhereExpString("p"));

    //    queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

    //    return queryable;
    //}

    public Pagination<TOutput> GetPagination(IQueryable<TEntity> queryable, Pagination<TOutput> pagination, Expression<Func<TEntity, TOutput>> selector)
    {
        int n = (pagination.Page - 1) * pagination.PageSize;
        List<TOutput> onePageOfData = queryable.Skip(n).Take(pagination.PageSize).Select(selector).ToList();

        return new Pagination<TOutput>
        {
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalItemCount = pagination.TotalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<TOutput>> GetPaginationAsync(IQueryable<TEntity> queryable, Pagination<TOutput> pagination, Expression<Func<TEntity, TOutput>> selector)
    {
        int n = (pagination.Page - 1) * pagination.PageSize;
        List<TOutput> onePageOfData = await queryable.Skip(n).Take(pagination.PageSize).Select(selector).ToListAsync();

        return new Pagination<TOutput>
        {
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalItemCount = pagination.TotalItemCount,
            Items = onePageOfData
        };
    }
}
