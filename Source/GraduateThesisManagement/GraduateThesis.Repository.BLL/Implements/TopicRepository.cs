using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class TopicRepository : AsyncSubRepository<Topic, TopicInput, TopicOutput, string>, ITopicRepository
{
    private HufiGraduateThesisContext _context;

    internal TopicRepository(HufiGraduateThesisContext context) 
        : base(context, context.Topics)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new TopicOutput
        {
           Id= s.Id,
           Name= s.Name,
           Description= s.Description,
           CreatedAt = s.CreatedAt,
           UpdatedAt = s.UpdatedAt,
           DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new TopicOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetMapperToCreate(TopicInput input, Topic entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    protected override void SetMapperToUpdate(TopicInput input, Topic entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetOutputMapper(Topic entity, TopicOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }
}
