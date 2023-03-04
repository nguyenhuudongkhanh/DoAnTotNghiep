using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class TrainingFormRepository : AsyncSubRepository<TrainingForm, TrainingFormInput, TrainingFormOutput, string>, ITrainingFormRepository
{
    private HufiGraduateThesisContext _context;

    internal TrainingFormRepository(HufiGraduateThesisContext context) 
        : base(context, context.TrainingForms)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Theses);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new TrainingFormOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new TrainingFormOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetMapperToCreate(TrainingFormInput input, TrainingForm entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(TrainingFormInput input, TrainingForm entity)
    {
        entity.Name = input.Name;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(TrainingForm entity, TrainingFormOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }
}
