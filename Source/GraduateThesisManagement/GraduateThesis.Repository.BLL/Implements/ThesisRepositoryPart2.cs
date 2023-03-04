using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Repository.BLL.Consts;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public partial class ThesisRepository
{
    public async Task<Pagination<ThesisOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
                    .Where(t => t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
                    .Where(t => t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                Year = s.Year,
                Semester = s.Semester,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                Notes = s.Notes
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfThesesInprAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses
            .Include(i => i.Lecture).Include(i => i.ThesisGroup)
            .Where(t => t.StatusId == ThesisStatusConsts.InProgress && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                ThesisSupervisor = _context.ThesisSupervisors.Include(i => i.Lecturer)
                    .Where(ts => ts.ThesisId == s.Id && ts.Lecturer.IsDeleted == false)
                    .Select(ts => new FacultyStaffOutput
                    {
                        Id = ts.Lecturer.Id,
                        Surname = ts.Lecturer.Surname,
                        Name = ts.Lecturer.Name,
                    }).SingleOrDefault(),
                CriticalLecturer = _context.CounterArgumentResults.Include(i => i.Lecturer)
                    .Where(c => c.ThesisId == s.Id && c.Lecturer.IsDeleted == false)
                    .Select(c => new FacultyStaffOutput
                    {
                        Id = c.Lecturer.Id,
                        Surname = c.Lecturer.Surname,
                        Name = c.Lecturer.Name,
                    }).SingleOrDefault(),
                ThesisGroup = new ThesisGroupOutput
                {
                    Students = _context.ThesisGroupDetails.Include(i => i.Student)
                        .Where(i => i.StudentThesisGroupId == s.ThesisGroupId && i.Student.IsDeleted == false)
                        .Select(st => new StudentOutput
                        {
                            Id = st.StudentId,
                            Surname = st.Student.Surname,
                            Name = st.Student.Name
                        }).ToList()
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(string lecturerId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == 4 && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                Year = s.Year,
                Semester = s.Semester,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                Notes = s.Notes
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnToAssignSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        List<string> thesisIds = await _context.ThesisSupervisors.Include(i => i.Thesis)
            .Where(ts => ts.Thesis.StatusId == ThesisStatusConsts.Approved && ts.Thesis.IsDeleted == false)
            .Select(s => s.Thesis.Id).ToListAsync();

        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .WhereBulkNotContains(thesisIds, t => t.Id);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAssignedSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<ThesisSupervisor> queryable = _context.ThesisSupervisors
            .Include(i => i.Thesis).Include(i => i.Thesis.Lecture)
            .Where(ts => ts.Thesis.StatusId == ThesisStatusConsts.Approved && ts.Thesis.IsDeleted == false && ts.Thesis.Lecture.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(t => t.Thesis.Year.Contains(keyword)); break;
                case "LectureName":
                    {
                        int lastIndexOf = keyword.Trim(' ').LastIndexOf(" ");
                        if (lastIndexOf > 0)
                        {
                            string name = keyword.Substring(lastIndexOf).Trim(' ');
                            string surname = keyword.Replace(name, "").Trim(' ');
                            queryable = queryable.Where(t => t.Thesis.Lecture.Surname.Contains(surname) && t.Thesis.Lecture.Name.Contains(name));
                        }
                        else
                        {
                            queryable = queryable.Where(t => t.Thesis.Lecture.Name.Contains(keyword));
                        }
                        break;
                    }
                case "Semester":
                    {
                        int semester = 0;
                        if (int.TryParse(keyword, out semester))
                            queryable = queryable.Where(t => t.Thesis.Semester == semester);

                        break;
                    }
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                Semester = s.Thesis.Semester,
                Year = s.Thesis.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Thesis.Lecture.Id,
                    Surname = s.Thesis.Lecture.Surname,
                    Name = s.Thesis.Lecture.Name
                },
                ThesisSupervisor = new FacultyStaffOutput
                {
                    Id = s.Lecturer.Id,
                    Surname = s.Lecturer.Surname,
                    Name = s.Lecturer.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnToAssignCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        List<string> thesisIds = await _context.CounterArgumentResults.Include(i => i.Thesis)
            .Where(ts => ts.Thesis.StatusId == ThesisStatusConsts.Submitted && ts.Thesis.IsDeleted == false)
            .Select(s => s.Thesis.Id).ToListAsync();

        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture).Include(i => i.ThesisSupervisor)
            .Where(t => t.StatusId == ThesisStatusConsts.Submitted && t.ThesisGroupId != null && t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .Where(t => t.ThesisSupervisor.IsCompleted == true)
            .WhereBulkNotContains(thesisIds, t => t.Id);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAssignedCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<CounterArgumentResult> queryable = _context.CounterArgumentResults
            .Include(i => i.Thesis).Include(i => i.Thesis.Lecture)
            .Where(ts => ts.Thesis.StatusId == ThesisStatusConsts.Submitted && ts.Thesis.IsDeleted == false && ts.Thesis.Lecture.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(t => t.Thesis.Year.Contains(keyword)); break;
                case "LectureName":
                    {
                        int lastIndexOf = keyword.Trim(' ').LastIndexOf(" ");
                        if (lastIndexOf > 0)
                        {
                            string name = keyword.Substring(lastIndexOf).Trim(' ');
                            string surname = keyword.Replace(name, "").Trim(' ');
                            queryable = queryable.Where(t => t.Thesis.Lecture.Surname.Contains(surname) && t.Thesis.Lecture.Name.Contains(name));
                        }
                        else
                        {
                            queryable = queryable.Where(t => t.Thesis.Lecture.Name.Contains(keyword));
                        }
                        break;
                    }
                case "Semester":
                    {
                        int semester = 0;
                        if (int.TryParse(keyword, out semester))
                            queryable = queryable.Where(t => t.Thesis.Semester == semester);

                        break;
                    }
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                Semester = s.Thesis.Semester,
                Year = s.Thesis.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Thesis.Lecture.Id,
                    Surname = s.Thesis.Lecture.Surname,
                    Name = s.Thesis.Lecture.Name
                },
                CriticalLecturer = new FacultyStaffOutput
                {
                    Id = s.Lecturer.Id,
                    Surname = s.Lecturer.Surname,
                    Name = s.Lecturer.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnToAssignCmteAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        List<string> thesisIds = await _context.ThesisCommitteeResults
            .Where(tc => tc.Thesis.StatusId == ThesisStatusConsts.Submitted && tc.Thesis.IsDeleted == false)
            .Select(tc => tc.Thesis.Id).ToListAsync();

        IQueryable<Thesis> queryable = _context.Theses
            .Include(i => i.Lecture).Include(i => i.ThesisGroup).Include(i => i.CommitteeMemberResults)
            .Where(t => t.StatusId == ThesisStatusConsts.Submitted && t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .Where(t => t.CounterArgumentResult.IsCompleted == true)
            .WhereBulkNotContains(thesisIds, t => t.Id);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable
            .Join(
                _context.ThesisSupervisors.Include(i => i.Lecturer).Where(ts => ts.Lecturer.IsDeleted == false),
                thesis => thesis.Id,
                supervisor => supervisor.ThesisId,
                (thesis, supervisor) => new ThesisOutput
                {
                    Id = thesis.Id,
                    Name = thesis.Name,
                    Lecturer = new FacultyStaffOutput
                    {
                        Id = thesis.Lecture.Id,
                        Surname = thesis.Lecture.Surname,
                        Name = thesis.Lecture.Name
                    },
                    ThesisSupervisor = new FacultyStaffOutput
                    {
                        Id = supervisor.Lecturer.Id,
                        Surname = supervisor.Lecturer.Surname,
                        Name = supervisor.Lecturer.Name
                    }
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
                    }
                }
            ).CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Join(
                _context.ThesisSupervisors.Include(i => i.Lecturer).Where(ts => ts.Lecturer.IsDeleted == false),
                thesis => thesis.Id,
                supervisor => supervisor.ThesisId,
                (thesis, supervisor) => new ThesisOutput
                {
                    Id = thesis.Id,
                    Name = thesis.Name,
                    Lecturer = new FacultyStaffOutput
                    {
                        Id = thesis.Lecture.Id,
                        Surname = thesis.Lecture.Surname,
                        Name = thesis.Lecture.Name
                    },
                    ThesisSupervisor = new FacultyStaffOutput
                    {
                        Id = supervisor.Lecturer.Id,
                        Surname = supervisor.Lecturer.Surname,
                        Name = supervisor.Lecturer.Name
                    }
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
                    }
                }
            ).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfFinishedThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses
            .Include(i => i.Lecture).Include(i => i.ThesisGroup)
            .Where(t => t.StatusId == ThesisStatusConsts.Finished && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfSubmdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses
            .Include(i => i.Lecture).Include(i => i.ThesisGroup)
            .Where(t => t.StatusId == ThesisStatusConsts.Submitted && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                ThesisSupervisor = _context.ThesisSupervisors.Include(i => i.Lecturer)
                    .Where(ts => ts.ThesisId == s.Id && ts.Lecturer.IsDeleted == false)
                    .Select(ts => new FacultyStaffOutput
                    {
                        Id = ts.Lecturer.Id,
                        Surname = ts.Lecturer.Surname,
                        Name = ts.Lecturer.Name,
                    }).SingleOrDefault(),
                CriticalLecturer = _context.CounterArgumentResults.Include(i => i.Lecturer)
                    .Where(c => c.ThesisId == s.Id && c.Lecturer.IsDeleted == false)
                    .Select(c => new FacultyStaffOutput
                    {
                        Id = c.Lecturer.Id,
                        Surname = c.Lecturer.Surname,
                        Name = c.Lecturer.Name,
                    }).SingleOrDefault(),
                ThesisGroup = new ThesisGroupOutput
                {
                    Students = _context.ThesisGroupDetails.Include(i => i.Student)
                        .Where(i => i.StudentThesisGroupId == s.ThesisGroupId && i.Student.IsDeleted == false)
                        .Select(st => new StudentOutput
                        {
                            Id = st.StudentId,
                            Surname = st.Student.Surname,
                            Name = st.Student.Name
                        }).ToList()
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnToSupvAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<ThesisSupervisor> queryable = _context.ThesisSupervisors
            .Include(i => i.Thesis).Include(i => i.Thesis.ThesisGroup)
            .Where(ts => ts.LecturerId == lecturerId && (ts.Thesis.StatusId == ThesisStatusConsts.InProgress || ts.Thesis.StatusId == ThesisStatusConsts.Submitted) && ts.Thesis.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(ts => ts.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(ts => ts.Thesis.Year.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(ts => ts.Thesis.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(ts => ts.Thesis.Id); break;
                case "Name": queryable = queryable.OrderBy(ts => ts.Thesis.Name); break;
                case "Year": queryable = queryable.OrderBy(ts => ts.Thesis.Year); break;
                case "CreatedAt": queryable = queryable.OrderBy(t => t.Thesis.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(ts => ts.Thesis.Id); break;
                case "Name": queryable = queryable.OrderByDescending(ts => ts.Thesis.Name); break;
                case "Year": queryable = queryable.OrderByDescending(ts => ts.Thesis.Year); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(t => t.Thesis.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                ThesisGroup = new ThesisGroupOutput
                {
                    Students = _context.ThesisGroupDetails.Include(i => i.Student)
                        .Where(i => i.StudentThesisGroupId == s.Thesis.ThesisGroupId && i.Student.IsDeleted == false)
                        .Select(st => new StudentOutput
                        {
                            Id = st.StudentId,
                            Surname = st.Student.Surname,
                            Name = st.Student.Name
                        }).ToList()
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnToCriticizeAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<CounterArgumentResult> queryable = _context.CounterArgumentResults
            .Include(i => i.Thesis).Include(i => i.Thesis.ThesisGroup)
            .Where(ts => ts.LecturerId == lecturerId && ts.Thesis.StatusId == ThesisStatusConsts.Submitted && ts.Thesis.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(ts => ts.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(ts => ts.Thesis.Year.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(ts => ts.Thesis.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(ts => ts.Thesis.Id); break;
                case "Name": queryable = queryable.OrderBy(ts => ts.Thesis.Name); break;
                case "Year": queryable = queryable.OrderBy(ts => ts.Thesis.Year); break;
                case "CreatedAt": queryable = queryable.OrderBy(t => t.Thesis.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(ts => ts.Thesis.Id); break;
                case "Name": queryable = queryable.OrderByDescending(ts => ts.Thesis.Name); break;
                case "Year": queryable = queryable.OrderByDescending(ts => ts.Thesis.Year); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(t => t.Thesis.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                ThesisGroup = new ThesisGroupOutput
                {
                    Students = _context.ThesisGroupDetails.Include(i => i.Student)
                        .Where(i => i.StudentThesisGroupId == s.Thesis.ThesisGroupId && i.Student.IsDeleted == false)
                        .Select(st => new StudentOutput
                        {
                            Id = st.StudentId,
                            Surname = st.Student.Surname,
                            Name = st.Student.Name
                        }).ToList()
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }
}