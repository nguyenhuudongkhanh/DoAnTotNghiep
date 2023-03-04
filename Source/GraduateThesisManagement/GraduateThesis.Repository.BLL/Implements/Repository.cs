using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using Microsoft.AspNetCore.Hosting;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class Repository : IRepository
{
    private HufiGraduateThesisContext _context;
    private IHostingEnvironment _hostingEnvironment;
    private IEmailService _emailService;
    private IFileManager _fileManager;

    public Repository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService, IFileManager fileManager)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _emailService = emailService;
        _fileManager = fileManager;
    }

    private bool disposedValue;

    public IStudentRepository StudentRepository => new StudentRepository(_context, _hostingEnvironment, _emailService, _fileManager);

    public IStudentClassRepository StudentClassRepository => new StudentClassRepository(_context);

    public IThesisRepository ThesisRepository => new ThesisRepository(_context, _hostingEnvironment, _emailService, _fileManager);

    public ITopicRepository TopicRepository => new TopicRepository(_context);

    public IThesisGroupRepository ThesisGroupRepository => new ThesisGroupRepository(_context);

    public IFacultyRepository FacultyRepository => new FacultyRepository(_context);

    public IThesisCommitteeRepository ThesisCommitteeRepository => new ThesisCommitteeRepository(_context);

    public IFacultyStaffRepository FacultyStaffRepository => new FacultyStaffRepository(_context, _hostingEnvironment, _emailService, _fileManager);

    public ITrainingFormRepository TrainingFormRepository => new TrainingFormRepository(_context);

    public ITrainingLevelRepository TrainingLevelRepository => new TrainingLevelRepository(_context);

    public ISpecializationRepository SpecializationRepository => new SpecializationRepository(_context);

    public IAppRoleRepository AppRolesRepository => new AppRoleRepository(_context);

    public IThesisRevisionRepository ThesisRevisionRepository => new ThesisRevisionRepository(_context);

    public IAppPageRepository AppPageRepository => new AppPageRepository(_context);

    public ISystemRepository SystemRepository => new SystemRepository(_context);

    public IReportRepository ReportRepository => new ReportRepository(_context);

    public ICouncilRepository CouncilRepository => new CouncilRepository(_context);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
