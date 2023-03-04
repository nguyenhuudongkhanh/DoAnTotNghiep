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

public class ThesisCommitteeRepository : AsyncSubRepository<ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput, string>, IThesisCommitteeRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisCommitteeRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisCommittees)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Council);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CouncilId = s.CouncilId,
            Council = new CouncilOutput
            {
                Id = s.Council.Id,
                Name = s.Council.Name
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        ListSelector = ListSelector;
        SingleSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Council = new CouncilOutput
            {
                Id = s.Council.Id,
                Name = s.Council.Name,
                Description = s.Council.Description,
                Semester = s.Council.Semester,
                Year = s.Council.Year
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }

    protected override void SetMapperToCreate(ThesisCommitteeInput input, ThesisCommittee entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CouncilId = input.CouncilId;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(ThesisCommitteeInput input, ThesisCommittee entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CouncilId = input.CouncilId;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(ThesisCommittee entity, ThesisCommitteeOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    public override async Task<ThesisCommitteeOutput> GetAsync(string id)
    {
        ThesisCommitteeOutput thesisCommittee = await base.GetAsync(id);

        if (thesisCommittee != null)
        {
            thesisCommittee.Theses = await _context.ThesisCommitteeResults
                .Include(i => i.Thesis).Include(i => i.Thesis.Lecture)
                .Where(tc => tc.ThesisCommitteeId == thesisCommittee.Id && tc.Thesis.IsDeleted == false)
                .Join(
                    _context.ThesisSupervisors.Include(i => i.Lecturer).Where(ts => ts.Lecturer.IsDeleted == false),
                    committeeRes => committeeRes.ThesisId,
                    supervisor => supervisor.ThesisId,
                    (committeeRes, supervisor) => new ThesisOutput
                    {
                        Id = committeeRes.ThesisId,
                        Name = committeeRes.Thesis.Name,
                        ThesisGroupId = committeeRes.Thesis.ThesisGroupId,
                        Lecturer = new FacultyStaffOutput
                        {
                            Id = committeeRes.Thesis.Id,
                            Surname = committeeRes.Thesis.Lecture.Surname,
                            Name = committeeRes.Thesis.Lecture.Name
                        },
                        ThesisSupervisor = new FacultyStaffOutput
                        {
                            Id = supervisor.Lecturer.Id,
                            Surname = supervisor.Lecturer.Surname,
                            Name = supervisor.Lecturer.Name
                        },
                        StatusId = committeeRes.Thesis.StatusId
                    }
                ).Join(
                    _context.CounterArgumentResults.Include(i => i.Lecturer).Where(c => c.Lecturer.IsDeleted == false),
                    combined => combined.Id,
                    criticalLecturer => criticalLecturer.ThesisId,
                    (combined, criticalLecturer) => new ThesisOutput
                    {
                        Id = combined.Id,
                        Name = combined.Name,
                        Lecturer = combined.Lecturer,
                        ThesisSupervisor = combined.ThesisSupervisor,
                        CriticalLecturer = new FacultyStaffOutput
                        {
                            Id = criticalLecturer.Lecturer.Id,
                            Surname = criticalLecturer.Lecturer.Surname,
                            Name = criticalLecturer.Lecturer.Name
                        },
                        ThesisGroupId = combined.ThesisGroupId,
                        StatusId = combined.StatusId
                    }
                ).ToListAsync();

            thesisCommittee.Members = await _context.CommitteeMembers
                .Where(cm => cm.ThesisCommitteeId == thesisCommittee.Id)
                .Select(s => new CommitteeMemberOutput
                {
                    MemberId = s.MemberId,
                    Surname = s.Member.Surname,
                    Name = s.Member.Name,
                    Email = s.Member.Email,
                    Titles = s.Titles
                }).ToListAsync();
        }

        return thesisCommittee;
    }

    public async Task<DataResponse> AddMemberAsync(CommitteeMemberInput input)
    {
        bool checkExists = await _context.CommitteeMembers
         .AnyAsync(t => t.MemberId == input.MemberId && t.ThesisCommitteeId == input.CommitteeId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Thành viên đã được thêm vào tiểu ban!"
            };

        CommitteeMember committeeMember = new CommitteeMember
        {
            Id = UidHelper.GetShortUid(),
            ThesisCommitteeId = input.CommitteeId,
            MemberId = input.MemberId,
            Titles = input.Titles
        };

        await _context.CommitteeMembers.AddAsync(committeeMember);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Thêm giảng viên vào tiểu ban thành công!"
        };
    }

    public async Task<DataResponse> DeleteMemberAsync(string committeeId, string memberId)
    {
        CommitteeMember committeeMember = await _context.CommitteeMembers
            .Where(t => t.ThesisCommitteeId == committeeId && t.MemberId == memberId)
                       .SingleOrDefaultAsync();

        if (committeeMember == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy thành viên này trong tiểu ban!"
            };

        _context.CommitteeMembers.Remove(committeeMember);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa thành công!"
        };
    }

    public async Task<DataResponse> AddThesisAsync(string committeeId, string thesisId)
    {
        bool checkExists = await _context.ThesisCommitteeResults
         .AnyAsync(t => t.ThesisId == thesisId && t.ThesisCommitteeId == committeeId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đề tài đã được thêm vào tiểu ban!"
            };

        ThesisCommitteeResult committeeResult = new ThesisCommitteeResult
        {
            ThesisCommitteeId = committeeId,
            ThesisId = thesisId,
        };

        await _context.ThesisCommitteeResults.AddAsync(committeeResult);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Thêm giảng viên vào tiểu ban thành công!"
        };
    }

    public async Task<DataResponse> DeleteThesisAsync(string committeeId, string thesisId)
    {
        ThesisCommitteeResult committeeResult = await _context.ThesisCommitteeResults
            .Where(tc => tc.ThesisCommitteeId == committeeId && tc.ThesisId == thesisId)
            .SingleOrDefaultAsync();

        if (committeeResult == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài này trong tiểu ban!"
            };

        _context.ThesisCommitteeResults.Remove(committeeResult);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa thành công!"
        };
    }

    public async Task<Pagination<ThesisCommitteeOutput>> GetPaginationAsync(string lectureId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<CommitteeMember> queryable = _context.CommitteeMembers
            .Include(i => i.ThesisCommittee).Include(i => i.ThesisCommittee.Council)
            .Where(cm => cm.MemberId == lectureId && cm.ThesisCommittee.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(cm => cm.ThesisCommittee.Id.Contains(keyword) || cm.ThesisCommittee.Name.Contains(keyword) || cm.ThesisCommittee.Council.Name.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(cm => cm.ThesisCommittee.Id.Contains(keyword) || cm.ThesisCommittee.Name.Contains(keyword) || cm.ThesisCommittee.Council.Name.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(cm => cm.ThesisCommittee.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(cm => cm.ThesisCommittee.Name.Contains(keyword)); break;
                case "CouncilName": queryable = queryable.Where(cm => cm.ThesisCommittee.Council.Name.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(cm => cm.ThesisCommittee.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(cm => cm.ThesisCommittee.Id); break;
                case "Surname": queryable = queryable.OrderBy(cm => cm.ThesisCommittee.Name); break;
                case "Name": queryable = queryable.OrderBy(cm => cm.ThesisCommittee.Council.Name); break;
                case "CreatedAt": queryable = queryable.OrderBy(cm => cm.ThesisCommittee.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(cm => cm.ThesisCommittee.Id); break;
                case "Surname": queryable = queryable.OrderByDescending(cm => cm.ThesisCommittee.Name); break;
                case "Name": queryable = queryable.OrderByDescending(cm => cm.ThesisCommittee.Council.Name); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(cm => cm.ThesisCommittee.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisCommitteeOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisCommitteeOutput
            {
                Id = s.ThesisCommittee.Id,
                Name = s.ThesisCommittee.Name,
                Description = s.ThesisCommittee.Description,
                Council = new CouncilOutput
                {
                    Id = s.ThesisCommittee.Council.Id,
                    Name = s.ThesisCommittee.Council.Name
                },
                CreatedAt = s.ThesisCommittee.CreatedAt,
                UpdatedAt = s.ThesisCommittee.UpdatedAt
            }).ToListAsync();

        return new Pagination<ThesisCommitteeOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }
}