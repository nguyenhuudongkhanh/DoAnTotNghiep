using System;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IRepository : IDisposable
{
    ISystemRepository SystemRepository { get; }
    IStudentRepository StudentRepository { get; }
    IStudentClassRepository StudentClassRepository { get; }
    IThesisRepository ThesisRepository { get; }
    ITopicRepository TopicRepository { get; }
    IThesisCommitteeRepository ThesisCommitteeRepository { get; }
    IThesisGroupRepository ThesisGroupRepository { get; }
    IFacultyRepository FacultyRepository { get; }
    IFacultyStaffRepository FacultyStaffRepository { get; }
    ITrainingFormRepository TrainingFormRepository { get; }
    ITrainingLevelRepository TrainingLevelRepository { get; }
    ISpecializationRepository SpecializationRepository { get; }
    IAppRoleRepository AppRolesRepository { get; }
    IThesisRevisionRepository ThesisRevisionRepository { get; }
    IAppPageRepository AppPageRepository { get; }
    IReportRepository ReportRepository { get; }
    ICouncilRepository CouncilRepository { get; }
}
