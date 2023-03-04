using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class TrainingLevelRepository : AsyncSubRepository<TrainingLevel, TrainingLevelInput, TrainingLevelOutput, string>, ITrainingLevelRepository
{
    private HufiGraduateThesisContext _context;

    internal TrainingLevelRepository(HufiGraduateThesisContext context)
        : base(context, context.TrainingLevels)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.Theses);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new TrainingLevelOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new TrainingLevelOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetOutputMapper(TrainingLevel entity, TrainingLevelOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToCreate(TrainingLevelInput input, TrainingLevel entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(TrainingLevelInput input, TrainingLevel entity)
    {
        entity.Name = input.Name;
        entity.UpdatedAt = DateTime.Now;
    }
}
