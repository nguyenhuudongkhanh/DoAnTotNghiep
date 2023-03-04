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

public class AppRoleRepository : AsyncSubRepository<AppRole, AppRoleInput, AppRoleOutput, string>, IAppRoleRepository
{
    private HufiGraduateThesisContext _context;

    public AppRoleRepository(HufiGraduateThesisContext context)
        :base(context, context.AppRoles)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {

    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppRoleOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new AppRoleOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetOutputMapper(AppRole entity, AppRoleOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToUpdate(AppRoleInput input, AppRole entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(AppRoleInput input, AppRole entity)
    {
        entity.Id = UidHelper.GetMicrosoftUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    public override async Task<AppRoleOutput> GetAsync(string id)
    {
        AppRoleOutput appRole = await base.GetAsync(id);
        if(appRole != null)
        {
            appRole.AppPages = await _context.AppRoleMappings.Include(i => i.Page)
                .Where(rm => rm.RoleId == appRole.Id && rm.Page.IsDeleted == false)
                .Select(s => new AppPageOutput
                {
                    Id = s.Page.Id,
                    PageName = s.Page.PageName,
                    ControllerName = s.Page.ControllerName,
                    ActionName = s.Page.ActionName
                }).ToListAsync();
        }

        return appRole;
    }

    public async Task<DataResponse> AddPageAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkPageExists = await _context.AppPages
            .AnyAsync(p => p.Id == input.PageId && p.IsDeleted == false);
        if (!checkPageExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy trang có Id này!"
            };

        bool checkExists = await _context.AppRoleMappings
            .AnyAsync(rm => rm.RoleId == input.RoleId && rm.PageId == input.PageId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đã gán quyền với trang này, không thể thực hiện gán trùng!"
            };

        AppRoleMapping appRoleMapping = new AppRoleMapping
        {
            RoleId = input.RoleId,
            PageId = input.PageId,
            CreatedAt = DateTime.Now
        };

        await _context.AppRoleMappings.AddAsync(appRoleMapping);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Thêm trang thành công!"
        };
    }

    public async Task<DataResponse> RemovePageAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkPageExists = await _context.AppPages
            .AnyAsync(p => p.Id == input.PageId && p.IsDeleted == false);
        if (!checkPageExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy trang có Id này!"
            };

        AppRoleMapping appRoleMapping = await _context.AppRoleMappings
            .FindAsync(input.RoleId, input.PageId);

        if (appRoleMapping == null)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể thu hồi quyền vì không tồn tại trong hệ thống!"
            };

        _context.AppRoleMappings.Remove(appRoleMapping);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa trang thành công!"
        };
    }

    public async Task<DataResponse> AddPagesAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        string[] pageIds = input.PageIds.Split(new char[] { ';' });
        List<AppRoleMapping> roleMappings = new List<AppRoleMapping>();
        foreach(string pageId in pageIds)
        {
            roleMappings.Add(new AppRoleMapping { RoleId = input.RoleId, PageId = pageId });
        }

        await _context.AppRoleMappings.AddRangeAsync(roleMappings);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Thêm các trang đã chọn thành công!"
        };
    }

    public async Task<DataResponse> RemovePagesAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        string[] pageIds = input.PageIds.Split(new char[] { ';' });
        List<AppRoleMapping> roleMappings = await _context.AppRoleMappings
            .Where(rm => rm.RoleId == input.RoleId && pageIds.Any(p => p == rm.PageId))
            .ToListAsync();

        _context.AppRoleMappings.RemoveRange(roleMappings);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa các trang đã chọn thành công!"
        };
    }

    public async Task<Pagination<AppRoleOutput>> GetPgnHasNtUserId(string userId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        List<string> roleIds = await _context.AppUserRoles
            .Where(ur => ur.UserId == userId && ur.Role.IsDeleted == false)
            .Select(s => s.RoleId).ToListAsync();

        IQueryable<AppRole> queryable = _context.AppRoles
            .Where(r => r.IsDeleted == false).WhereBulkNotContains(roleIds, r => r.Id);

        if (!string.IsNullOrEmpty(keyword))
        {
            queryable = queryable.Where(r => r.Id.Contains(keyword) || r.Name.Contains(keyword));
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(p => p.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(r => r.Id); break;
                case "Name": queryable = queryable.OrderBy(r => r.Name); break;
                case "CreatedAt": queryable = queryable.OrderBy(r => r.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(r => r.Id); break;
                case "Name": queryable = queryable.OrderByDescending(r => r.Name); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(r => r.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<AppRoleOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new AppRoleOutput
            {
                Id = s.Id,
                Name = s.Name,
            }).ToListAsync();

        return new Pagination<AppRoleOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }
}
