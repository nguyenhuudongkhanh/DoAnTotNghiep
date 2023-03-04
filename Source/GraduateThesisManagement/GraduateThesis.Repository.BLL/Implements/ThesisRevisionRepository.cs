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
using System.Web;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisRevisionRepository : AsyncSubRepository<ThesisRevision, ThesisRevisionInput, ThesisRevisionOutput, string>, IThesisRevisionRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisRevisionRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisRevisions)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Thesis);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisRevisionOutput
        {
            Id = s.Id,
            Title = s.Title,
            Summary = s.Summary
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new ThesisRevisionOutput
        {
            Id = s.Id,
            Title = s.Title,
            Summary = s.Summary,
            DocumentFile = s.DocumentFile,
            PresentationFile = s.PresentationFile,
            PdfFile = s.PdfFile,
            SourceCode = s.SourceCode,
            Reviewed = s.Reviewed,
            LecturerComment = s.LecturerComment,
            Point = s.Point,
            Thesis = new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                StatusId = s.Thesis.StatusId
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    protected override void SetOutputMapper(ThesisRevision entity, ThesisRevisionOutput output)
    {
        output.Id = entity.Id;
        output.ThesisId = entity.ThesisId;
        output.Title = entity.Title;
    }

    protected override void SetMapperToUpdate(ThesisRevisionInput input, ThesisRevision entity)
    {
        entity.Title = entity.Title;
        entity.Summary = entity.Summary;
        entity.LecturerComment = entity.LecturerComment;
        entity.Point = entity.Point;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(ThesisRevisionInput input, ThesisRevision entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.ThesisId = input.ThesisId;
        entity.Title = input.Title;
        entity.Summary = input.Summary;
        entity.DocumentFile = input.DocumentFile;
        entity.PresentationFile = input.PresentationFile;
        entity.PdfFile = input.PdfFile;
        entity.SourceCode = input.SourceCode;
        entity.CreatedAt = DateTime.Now;
    }

    public async Task<List<ThesisRevisionOutput>> GetRevsByThesisIdAsync(string thesisId)
    {
        List<ThesisRevisionOutput> thesisRevisions = await _context.ThesisRevisions
            .Where(tr => tr.ThesisId == thesisId && tr.IsDeleted == false)
            .Select(s => new ThesisRevisionOutput
            {
                Id = s.Id,
                Title = s.Title,
                Summary = s.Summary,
                DocumentFile = s.DocumentFile,
                PresentationFile = s.PresentationFile,
                PdfFile = s.PdfFile,
                SourceCode = s.SourceCode,
                Reviewed = s.Reviewed,
                LecturerComment = s.LecturerComment,
                Point = s.Point,
                Thesis = new ThesisOutput
                {
                    Id = s.Thesis.Id,
                    Name = s.Thesis.Name,
                },
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            }).OrderByDescending(tr => tr.CreatedAt).ToListAsync();

        return thesisRevisions
            .Select(s => { s.LecturerComment = HttpUtility.HtmlDecode(s.LecturerComment); return s; })
            .ToList();
    }

    public async Task<DataResponse> ReviewRevisionAsync(ThesisRevReviewInput thesisRevReview)
    {
        ThesisRevision thesisRevision = await _context.ThesisRevisions
            .Where(tr => tr.Id == thesisRevReview.RevisionId && tr.IsDeleted == false).SingleOrDefaultAsync();

        if (thesisRevision == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy phiên bản có mã này!"
            };

        thesisRevision.Reviewed = true;
        thesisRevision.LecturerComment = HttpUtility.HtmlEncode(thesisRevReview.Comment);
        thesisRevision.Point = thesisRevReview.Point;

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã đánh giá cho quá trình thực hiện đề tài thành công!"
        };
    }

}
