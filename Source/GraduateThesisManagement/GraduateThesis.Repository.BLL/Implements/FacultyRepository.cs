using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class FacultyRepository : AsyncSubRepository<Faculty, FacultyInput, FacultyOutput, string>, IFacultyRepository
{
    private HufiGraduateThesisContext _context;

    public FacultyRepository(HufiGraduateThesisContext context)
        : base(context, context.Faculties)
    {
        _context = context;;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.FacultyStaffs);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        SimpleImportSelector = r => new Faculty
        {
            Id = UidHelper.GetShortUid(),
            Name = r[1] as string,
            Description = r[2] as string,
            CreatedAt = DateTime.Now
        };
    }

    protected override void SetMapperToCreate(FacultyInput input, Faculty entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(FacultyInput input, Faculty entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(Faculty entity, FacultyOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }
}
