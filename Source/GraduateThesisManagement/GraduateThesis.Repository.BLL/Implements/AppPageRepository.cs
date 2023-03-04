using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppPageRepository : AsyncSubRepository<AppPage, AppPageInput, AppPageOutput, string>, IAppPageRepository
{
    private HufiGraduateThesisContext _context;

    public AppPageRepository(HufiGraduateThesisContext context) 
        : base(context, context.AppPages)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppPageOutput
        {
            Id = s.Id,
            PageName = s.PageName,
            ControllerName = s.ControllerName,
            ActionName = s.ActionName
        };

        ListSelector = PaginationSelector;

        SingleSelector = s => new AppPageOutput
        {
            Id = s.Id,
            PageName = s.PageName,
            ControllerName = s.ControllerName,
            ActionName = s.ActionName,
            Area = s.Area,
            Path = s.Path,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }

    protected override void SetOutputMapper(AppPage entity, AppPageOutput output)
    {
        output.Id = entity.Id;
        output.PageName = entity.PageName;
    }

    protected override void SetMapperToUpdate(AppPageInput input, AppPage entity)
    {
        entity.PageName = input.PageName;
        entity.ControllerName = input.ControllerName;
        entity.ActionName = input.ActionName;
        entity.Area = input.Area;
        entity.Path = input.Path;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(AppPageInput input, AppPage entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.PageName = input.PageName;
        entity.ControllerName = input.ControllerName;
        entity.ActionName = input.ActionName;
        entity.Area = input.Area;
        entity.Path = input.Path;
        entity.CreatedAt = DateTime.Now;
    }

    public async Task<Pagination<AppPageOutput>> GetPgnHasRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        IQueryable<AppRoleMapping> queryable = _context.AppRoleMappings
            .Where(rm => rm.RoleId == roleId && rm.Page.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword))
        {
            queryable = queryable.Where(rm => rm.Page.Id.Contains(keyword) || rm.Page.PageName.Contains(keyword) || rm.Page.ControllerName.Contains(keyword) || rm.Page.ActionName.Contains(keyword));
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(rm => rm.Page.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(rm => rm.Page.Id); break;
                case "PageName": queryable = queryable.OrderBy(rm => rm.Page.PageName); break;
                case "ControllerName": queryable = queryable.OrderBy(rm => rm.Page.ControllerName); break;
                case "ActionName": queryable = queryable.OrderBy(rm => rm.Page.ActionName); break;
                case "Area": queryable = queryable.OrderBy(rm => rm.Page.Area); break;
                case "CreatedAt": queryable = queryable.OrderBy(rm => rm.Page.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(rm => rm.Page.Id); break;
                case "PageName": queryable = queryable.OrderBy(rm => rm.Page.PageName); break;
                case "ControllerName": queryable = queryable.OrderBy(rm => rm.Page.ControllerName); break;
                case "ActionName": queryable = queryable.OrderBy(rm => rm.Page.ActionName); break;
                case "Area": queryable = queryable.OrderBy(rm => rm.Page.Area); break;
                case "CreatedAt": queryable = queryable.OrderBy(rm => rm.Page.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<AppPageOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new AppPageOutput
            {
                Id = s.Page.Id,
                PageName = s.Page.PageName,
                ControllerName = s.Page.ControllerName,
                ActionName = s.Page.ActionName
            }).ToListAsync();

        return new Pagination<AppPageOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<AppPageOutput>> GetPgnHasNtRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        List<string> pageIds = await _context.AppRoleMappings
            .Where(rm => rm.RoleId == roleId && rm.Page.IsDeleted == false)
            .Select(s => s.PageId).ToListAsync();

        IQueryable<AppPage> queryable = _context.AppPages
            .Where(p => p.IsDeleted == false).WhereBulkNotContains(pageIds, p => p.Id);

        if (!string.IsNullOrEmpty(keyword))
        {
            queryable = queryable.Where(p => p.Id.Contains(keyword) || p.PageName.Contains(keyword) || p.ControllerName.Contains(keyword) || p.ActionName.Contains(keyword));
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(p => p.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(p => p.Id); break;
                case "PageName": queryable = queryable.OrderBy(p => p.PageName); break;
                case "ControllerName": queryable = queryable.OrderBy(p => p.ControllerName); break;
                case "ActionName": queryable = queryable.OrderBy(p => p.ActionName); break;
                case "Area": queryable = queryable.OrderBy(p => p.Area); break;
                case "CreatedAt": queryable = queryable.OrderBy(p => p.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(p => p.Id); break;
                case "PageName": queryable = queryable.OrderByDescending(p => p.PageName); break;
                case "ControllerName": queryable = queryable.OrderByDescending(p => p.ControllerName); break;
                case "ActionName": queryable = queryable.OrderByDescending(p => p.ActionName); break;
                case "Area": queryable = queryable.OrderByDescending(p => p.Area); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(p => p.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<AppPageOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new AppPageOutput
            {
                Id = s.Id,
                PageName = s.PageName,
                ControllerName = s.ControllerName,
                ActionName = s.ActionName
            }).ToListAsync();

        return new Pagination<AppPageOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }
}
