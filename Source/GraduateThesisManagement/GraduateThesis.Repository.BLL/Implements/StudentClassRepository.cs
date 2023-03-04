using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentClassRepository : AsyncSubRepository<StudentClass, StudentClassInput, StudentClassOutput, string>, IStudentClassRepository
{
    private HufiGraduateThesisContext _context;

    internal StudentClassRepository(HufiGraduateThesisContext context)
        :base(context, context.StudentClasses)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.Faculty);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentClassOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity= s.StudentQuantity,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentClassOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            FacultyId = s.FacultyId,
            Faculty = new FacultyOutput
            {
                Id = s.Faculty.Id,
                Name = s.Faculty.Name,
                Description = s.Faculty.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        SimpleImportSelector = r => new StudentClass
        {
            Id = r[0] as string,
            Name = r[1] as string,
            Description = r[2] as string,
            CreatedAt = DateTime.Now
        };
    }

    protected override void SetMapperToCreate(StudentClassInput input, StudentClass entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(StudentClassInput input, StudentClass entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(StudentClass entity, StudentClassOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }
}
