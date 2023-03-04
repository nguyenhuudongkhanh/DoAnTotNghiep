using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class CouncilRepository : AsyncSubRepository<Council, CouncilInput, CouncilOutput, string>, ICouncilRepository
{
    private HufiGraduateThesisContext _context;

    internal CouncilRepository(HufiGraduateThesisContext context)
        : base(context, context.Councils)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {

    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new CouncilOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Semester = s.Semester,
            Year = s.Year,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new CouncilOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Semester = s.Semester,
            Year = s.Year,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetMapperToCreate(CouncilInput input, Council entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Semester = input.Semester;
        entity.Year = input.Year;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(CouncilInput input, Council entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Semester = input.Semester;
        entity.Year = input.Year;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(Council entity, CouncilOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    public Task<byte[]> ExportAsync(string councilId)
    {
        return null;
    }
}