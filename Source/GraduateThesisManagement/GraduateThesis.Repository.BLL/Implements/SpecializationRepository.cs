using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class SpecializationRepository : AsyncSubRepository<Specialization, SpecializationInput, SpecializationOutput, string>, ISpecializationRepository
{
    internal SpecializationRepository(HufiGraduateThesisContext context) 
        : base(context, context.Specializations)
    {

    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new SpecializationOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;

        SingleSelector = s => new SpecializationOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetMapperToCreate(SpecializationInput input, Specialization entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(SpecializationInput input, Specialization entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(Specialization entity, SpecializationOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }
}