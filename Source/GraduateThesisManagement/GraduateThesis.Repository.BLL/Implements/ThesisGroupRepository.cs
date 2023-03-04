using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Consts;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisGroupRepository : AsyncSubRepository<ThesisGroup, ThesisGroupInput, ThesisGroupOutput, string>, IThesisGroupRepository
{
    private HufiGraduateThesisContext _context;

    public ThesisGroupRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisGroups)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Thesis);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisGroupOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            Notes = s.Notes
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new ThesisGroupOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            Notes = s.Notes,
            Thesis = new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                Description = s.Thesis.Description,
                Notes = s.Thesis.Notes,
                TopicId = s.Thesis.Notes,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
            }
        };
    }

    protected override void SetOutputMapper(ThesisGroup entity, ThesisGroupOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToUpdate(ThesisGroupInput input, ThesisGroup entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(ThesisGroupInput input, ThesisGroup entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    public async Task<Pagination<ThesisGroupOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture).Include(i => i.ThesisGroup)
            .Where(t => t.ThesisGroupId != null && t.IsDeleted == false && t.Lecture.IsDeleted == false && t.ThesisGroup.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(t => t.ThesisGroup.Id.Contains(keyword) || t.ThesisGroup.Name.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(t => t.ThesisGroup.Id.Contains(keyword) || t.ThesisGroup.Name.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.ThesisGroup.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.ThesisGroup.Name.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(t => t.ThesisGroup.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(t => t.ThesisGroup.Id); break;
                case "Name": queryable = queryable.OrderBy(t => t.ThesisGroup.Name); break;
                case "CreatedAt": queryable = queryable.OrderBy(t => t.ThesisGroup.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(t => t.ThesisGroup.Id); break;
                case "Name": queryable = queryable.OrderByDescending(t => t.ThesisGroup.Name); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(t => t.ThesisGroup.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisGroupOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisGroupOutput
            {
                Id = s.ThesisGroup.Id,
                Name = s.ThesisGroup.Name,
                Thesis = new ThesisOutput {
                    Id = s.Id, Name = s.Name,
                    Lecturer = new FacultyStaffOutput { Id = s.Lecture.Id, Surname = s.Lecture.Surname, Name = s.Lecture.Name }
                },
                Students = _context.ThesisGroupDetails.Include(i => i.Student)
                    .Where(gd => gd.StudentThesisGroupId == s.ThesisGroup.Id && gd.IsDeleted == false)
                    .Select(gd => new StudentOutput { Id = gd.Student.Id, Surname = gd.Student.Surname, Name = gd.Student.Name })
                    .ToList()
            }).ToListAsync();

        return new Pagination<ThesisGroupOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public override async Task<ThesisGroupOutput> GetAsync(string id)
    {
        ThesisGroupOutput thesisGroup = await base.GetAsync(id);

        thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
            .Where(s => s.StudentThesisGroupId == id && s.IsDeleted == false)
            .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
            .ToListAsync();

        return thesisGroup;
    }

    public async Task<ThesisGroupOutput> GetAsync(string studentId, string thesisId)
    {
        ThesisGroupOutput thesisGroup = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false)
            .Join(
                _context.ThesisGroups.Where(t => t.IsDeleted == false),
                groupDetail => groupDetail.StudentThesisGroupId,
                group => group.Id,
                (groupDetail, group) => group
            ).Join(
                _context.Theses.Where(t => t.Id == thesisId && t.IsDeleted == false),
                combined => combined.Id,
                thesis => thesis.Id,
                (combined, thesis) => new ThesisGroupOutput
                {
                    Id = combined.Id,
                    Name = combined.Name,
                    Description = combined.Description
                }
            ).SingleOrDefaultAsync();

        //thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
        //    .Where(s => s.StudentThesisGroupId == thesisGroup.Id && s.IsDeleted == false)
        //    .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
        //    .ToListAsync();

        return thesisGroup;
    }

    public async Task<List<ThesisGroupOutput>> GetListAsync(string studentId)
    {
        return await _context.ThesisGroupDetails.Include(i => i.StudentThesisGroup)
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false)
            .Select(s => new ThesisGroupOutput
            {
                Id = s.StudentThesisGroup.Id,
                Name = s.StudentThesisGroup.Name,
                Description = s.StudentThesisGroup.Description
            }).ToListAsync();
    }

    public async Task<DataResponse> JoinToGroupAsync(string studentId, string groupId)
    {
        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.StudentThesisGroupId == groupId && gd.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        thesisGroupDetail.StatusId = GroupStatusConsts.Joined;

        int studentQuantity = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentThesisGroupId == groupId && gd.StatusId == GroupStatusConsts.Joined && gd.IsDeleted == false)
            .CountAsync();

        ThesisGroup thesisGroup = await _context.ThesisGroups
            .Where(tg => tg.Id == groupId && tg.IsDeleted == false)
            .SingleOrDefaultAsync();

        thesisGroup.StudentQuantity = studentQuantity;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã vào nhóm thành công!"
        };
    }

    public async Task<DataResponse> DenyFromGroupAsync(string studentId, string groupId)
    {
        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.StudentThesisGroupId == groupId && gd.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        thesisGroupDetail.StatusId = GroupStatusConsts.Denied;

        int studentQuantity = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentThesisGroupId == groupId && gd.StatusId == GroupStatusConsts.Joined && gd.IsDeleted == false)
            .CountAsync();

        ThesisGroup thesisGroup = await _context.ThesisGroups
            .Where(tg => tg.Id == groupId && tg.IsDeleted == false)
            .SingleOrDefaultAsync();

        thesisGroup.StudentQuantity = studentQuantity;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã từ chối tham gia nhóm thành công!"
        };
    }

    public async Task<List<ThesisGroupDtOutput>> GetGroupsByStdntIdAsync(string studentId)
    {
        return await _context.ThesisGroupDetails.Include(i => i.StudentThesisGroup)
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false && gd.StudentThesisGroup.IsDeleted == false)
            .Join(
                _context.Theses.Where(t => t.IsDeleted == false),
                groupDetail => groupDetail.StudentThesisGroup.Id,
                thesis => thesis.ThesisGroupId,
                (groupDetail, thesis) => new ThesisGroupDtOutput
                {
                    ThesisId = thesis.Id,
                    ThesisName = thesis.Name,
                    GroupId = groupDetail.StudentThesisGroup.Id,
                    GroupName = groupDetail.StudentThesisGroup.Name,
                    GroupDescription = groupDetail.StudentThesisGroup.Description,
                    StudentQuantity = groupDetail.StudentThesisGroup.StudentQuantity,
                    RegistrationDate = groupDetail.StudentThesisGroup.CreatedAt,
                    GroupNotes = groupDetail.StudentThesisGroup.Notes,
                    MemberNotes = groupDetail.Notes,
                    IsLeader = groupDetail.IsLeader,
                    StatusId = groupDetail.StatusId
                }
            ).ToListAsync();
    }

    public async Task<bool> CheckIsLeaderAsync(string studentId, string groupId)
    {
        return await _context.ThesisGroupDetails
            .AnyAsync(gd => gd.StudentId == studentId && gd.StudentThesisGroupId == groupId && gd.IsLeader == true);
    }

    public async Task<DataResponse> UpdatePointsAsync(GroupPointInput input)
    {
        Thesis thesis = await _context.Theses.Where(t => t.Id == input.ThesisId && t.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài này!"
            };

        thesis.StatusId = ThesisStatusConsts.Finished;

        List<ThesisGroupDetail> groupDetails = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentThesisGroupId == input.GroupId && gd.StatusId == GroupStatusConsts.Submitted)
            .ToListAsync();

        foreach(ThesisGroupDetail groupDetail in groupDetails)
        {
            StudentGroupDtInput studentGroupDt = input.StudentPoints
                .Find(s => s.StudentId == groupDetail.StudentId);

            groupDetail.SupervisorPoint = studentGroupDt.SupvPoint;
            groupDetail.CtrArgPoint = studentGroupDt.CtrArgPoint;
            groupDetail.CommitteePoint = studentGroupDt.CmtePoint;
            groupDetail.FinalPoint = (groupDetail.SupervisorPoint + groupDetail.CtrArgPoint + groupDetail.CommitteePoint) / 3;
            if (groupDetail.FinalPoint >= (decimal)5.0)
                groupDetail.StatusId = GroupStatusConsts.Completed;
            else
                groupDetail.StatusId = GroupStatusConsts.Failed;
        }

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã cập nhật điểm cho nhóm thành công!"
        };
    }

    public async Task<List<StudentGroupDtOutput>> GetStndtGrpDtsAsync(string groupId)
    {
        return await _context.ThesisGroupDetails.Include(i => i.Student).Include(i => i.Status)
            .Where(gd => gd.StudentThesisGroupId == groupId && gd.Student.IsDeleted == false)
            .Where(gd => gd.StatusId != GroupStatusConsts.Denied && gd.StatusId != GroupStatusConsts.Pending)
            .Select(s => new StudentGroupDtOutput
            {
                StudentId = s.Student.Id,
                Surname = s.Student.Surname,
                Name = s.Student.Name,
                Email = s.Student.Email,
                IsLeader = s.IsLeader,
                StatusId = s.StatusId,
                StatusName = s.Status.Name,
                SupvPoint = s.SupervisorPoint,
                CtrArgPoint = s.CtrArgPoint,
                CmtePoint = s.CommitteePoint
            }).ToListAsync();
    }
}
