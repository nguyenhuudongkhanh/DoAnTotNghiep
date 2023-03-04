using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.Repository.DAL;

public partial class HufiGraduateThesisContext : DbContext
{
    public HufiGraduateThesisContext(DbContextOptions<HufiGraduateThesisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppPage> AppPages { get; set; }

    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppRoleMapping> AppRoleMappings { get; set; }

    public virtual DbSet<AppUserRole> AppUserRoles { get; set; }

    public virtual DbSet<CommitteeMember> CommitteeMembers { get; set; }

    public virtual DbSet<CommitteeMemberResult> CommitteeMemberResults { get; set; }

    public virtual DbSet<Council> Councils { get; set; }

    public virtual DbSet<CounterArgumentResult> CounterArgumentResults { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<FacultyStaff> FacultyStaffs { get; set; }

    public virtual DbSet<GroupStatus> GroupStatuses { get; set; }

    public virtual DbSet<ImplementationPlan> ImplementationPlans { get; set; }

    public virtual DbSet<MemberEvaluation> MemberEvaluations { get; set; }

    public virtual DbSet<MemberEvalutionPattern> MemberEvalutionPatterns { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentClass> StudentClasses { get; set; }

    public virtual DbSet<Thesis> Theses { get; set; }

    public virtual DbSet<ThesisCommittee> ThesisCommittees { get; set; }

    public virtual DbSet<ThesisCommitteeResult> ThesisCommitteeResults { get; set; }

    public virtual DbSet<ThesisGroup> ThesisGroups { get; set; }

    public virtual DbSet<ThesisGroupDetail> ThesisGroupDetails { get; set; }

    public virtual DbSet<ThesisRevision> ThesisRevisions { get; set; }

    public virtual DbSet<ThesisStatus> ThesisStatuses { get; set; }

    public virtual DbSet<ThesisSupervisor> ThesisSupervisors { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<TrainingForm> TrainingForms { get; set; }

    public virtual DbSet<TrainingLevel> TrainingLevels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppRoleMapping_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Area)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ControllerName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.PageName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Path)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FacultyRoles_ID ");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID ");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppRoleMapping>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PageId });

            entity.ToTable("AppRoleMapping");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PageId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Page).WithMany(p => p.AppRoleMappings)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppRoleMapping_AppPages_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.AppRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppRoleMapping_AppRoles_ID ");
        });

        modelBuilder.Entity<AppUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK_AppUserRoles_UserId");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AppUserRoles_AppRoles_ID ");

            entity.HasOne(d => d.User).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AppUserRoles_FacultyStaffs_ID");
        });

        modelBuilder.Entity<CommitteeMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CommitteeMembers_ThesisCommitteeId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.MemberId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThesisCommitteeId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titles)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Member).WithMany(p => p.CommitteeMembers)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_CommitteeMembers_FacultyStaffs_ID");

            entity.HasOne(d => d.ThesisCommittee).WithMany(p => p.CommitteeMembers)
                .HasForeignKey(d => d.ThesisCommitteeId)
                .HasConstraintName("FK_CommitteeMembers_ThesisCommittees_ID");
        });

        modelBuilder.Entity<CommitteeMemberResult>(entity =>
        {
            entity.HasKey(e => new { e.ThesisId, e.CommitteeMemberId }).HasName("PK_CouncilMembers_ID");

            entity.Property(e => e.ThesisId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommitteeMemberId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EvaluationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Point).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.CommitteeMember).WithMany(p => p.CommitteeMemberResults)
                .HasForeignKey(d => d.CommitteeMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommitteeMemberResults_CommitteeMembers_ID");

            entity.HasOne(d => d.Thesis).WithMany(p => p.CommitteeMemberResults)
                .HasForeignKey(d => d.ThesisId)
                .HasConstraintName("FK_CommitteeMemberResults_Theses_ID");
        });

        modelBuilder.Entity<Council>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Councils_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Year)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<CounterArgumentResult>(entity =>
        {
            entity.HasKey(e => e.ThesisId).HasName("PK_Counterargument_PK_Thesis_ID");

            entity.Property(e => e.ThesisId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Answers).HasColumnType("ntext");
            entity.Property(e => e.Conclusions).HasColumnType("ntext");
            entity.Property(e => e.Contents).HasColumnType("ntext");
            entity.Property(e => e.Defects).HasColumnType("ntext");
            entity.Property(e => e.LecturerId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Point).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PracticalResults).HasColumnType("ntext");
            entity.Property(e => e.ResearchMethods).HasColumnType("ntext");
            entity.Property(e => e.ScientificResults).HasColumnType("ntext");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.CounterArgumentResults)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("FK_Counterargument_FacultyStaffs_ID");

            entity.HasOne(d => d.Thesis).WithOne(p => p.CounterArgumentResult)
                .HasForeignKey<CounterArgumentResult>(d => d.ThesisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CounterArguments_Theses_ID");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Faculty_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<FacultyStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FacultyStaff_ID");

            entity.HasIndex(e => e.Email, "UQ__FacultyS__A9D10534A2288D2B").IsUnique();

            entity.HasIndex(e => e.Phone, "ux_fsphone_notnull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Avatar)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.CodeExpTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Degree).HasMaxLength(50);
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FacultyId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('BHyFWSywrk')");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValueSql("('Nam')");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(200);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyStaffs)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacultyStaffs_Faculties_ID");
        });

        modelBuilder.Entity<GroupStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GroupStatus_Id");

            entity.ToTable("GroupStatus");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<ImplementationPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ImplementationPlan_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Task)
                .IsRequired()
                .HasColumnType("ntext");
            entity.Property(e => e.ThesisId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Thesis).WithMany(p => p.ImplementationPlans)
                .HasForeignKey(d => d.ThesisId)
                .HasConstraintName("FK_ImplementationPlans_Theses_ID");
        });

        modelBuilder.Entity<MemberEvaluation>(entity =>
        {
            entity.HasKey(e => new { e.CommitteeMemberResultId, e.EvalutionPatternId }).HasName("PK_Evaluations_CommitteeMemberId");

            entity.Property(e => e.CommitteeMemberResultId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EvalutionPatternId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(450);
            entity.Property(e => e.Point).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.EvalutionPattern).WithMany(p => p.MemberEvaluations)
                .HasForeignKey(d => d.EvalutionPatternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberEvaluations_MemberEvalutionPatterns_ID");
        });

        modelBuilder.Entity<MemberEvalutionPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MemberEvalutionForms_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.MaxPoint).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ParentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Specialization_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Students_ID");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D105343DDCBD48").IsUnique();

            entity.HasIndex(e => e.Phone, "ux_sphone_notnull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(400);
            entity.Property(e => e.Avatar)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.CodeExpTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValueSql("(N'Nam')");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.StudentClassId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.StudentClass).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentClassId)
                .HasConstraintName("FK_Students_StudentClasses_ID");
        });

        modelBuilder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StudentClass_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.FacultyId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('BHyFWSywrk')");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Faculty).WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentClasses_Faculties_ID");
        });

        modelBuilder.Entity<Thesis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Thesis_ID");

            entity.HasIndex(e => e.ThesisGroupId, "ux_groupId_notnull")
                .IsUnique()
                .HasFilter("([ThesisGroupId] IS NOT NULL)");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("ntext");
            entity.Property(e => e.File).HasMaxLength(500);
            entity.Property(e => e.LectureId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.Notes).HasMaxLength(200);
            entity.Property(e => e.SpecializationId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");
            entity.Property(e => e.ThesisGroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TopicId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('rkrKd_KtQJemI')");
            entity.Property(e => e.TrainingFormId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrainingLevelId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('sHUaEg-qBhVh')");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Year)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Lecture).WithMany(p => p.Theses)
                .HasForeignKey(d => d.LectureId)
                .HasConstraintName("FK_Theses_FacultyStaffs_ID");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Theses)
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("FK_Theses_Specializations_ID");

            entity.HasOne(d => d.Status).WithMany(p => p.Theses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theses_ThesisStatus_Id");

            entity.HasOne(d => d.ThesisGroup).WithOne(p => p.Thesis)
                .HasForeignKey<Thesis>(d => d.ThesisGroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Theses_ThesisGroups_ID");

            entity.HasOne(d => d.Topic).WithMany(p => p.Theses)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theses_Topics_ID");

            entity.HasOne(d => d.TrainingForm).WithMany(p => p.Theses)
                .HasForeignKey(d => d.TrainingFormId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Theses_TrainingForms_ID");

            entity.HasOne(d => d.TrainingLevel).WithMany(p => p.Theses)
                .HasForeignKey(d => d.TrainingLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theses_TrainingLevels_ID");
        });

        modelBuilder.Entity<ThesisCommittee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Council_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CouncilId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Council).WithMany(p => p.ThesisCommittees)
                .HasForeignKey(d => d.CouncilId)
                .HasConstraintName("FK_ThesisCommittees_Councils_ID");
        });

        modelBuilder.Entity<ThesisCommitteeResult>(entity =>
        {
            entity.HasKey(e => e.ThesisId).HasName("PK_ThesisCommitteeResults_ThesisId");

            entity.Property(e => e.ThesisId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Conclusions).HasColumnType("ntext");
            entity.Property(e => e.Contents).HasColumnType("ntext");
            entity.Property(e => e.Point).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ThesisCommitteeId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ThesisCommittee).WithMany(p => p.ThesisCommitteeResults)
                .HasForeignKey(d => d.ThesisCommitteeId)
                .HasConstraintName("FK_ThesisCommitteeResults_ThesisCommittees_ID");

            entity.HasOne(d => d.Thesis).WithOne(p => p.ThesisCommitteeResult)
                .HasForeignKey<ThesisCommitteeResult>(d => d.ThesisId)
                .HasConstraintName("FK_ThesisCommitteeResults_Theses_ID");
        });

        modelBuilder.Entity<ThesisGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StudentThesisGroup_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Notes).HasColumnType("ntext");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<ThesisGroupDetail>(entity =>
        {
            entity.HasKey(e => new { e.StudentThesisGroupId, e.StudentId }).HasName("PK_StudentThesisGroupDetail_PK_StudentThesisGroup_ID");

            entity.Property(e => e.StudentThesisGroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommitteePoint).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CtrArgPoint).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.FinalPoint).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Notes).HasColumnType("ntext");
            entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");
            entity.Property(e => e.SupervisorPoint).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Status).WithMany(p => p.ThesisGroupDetails)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThesisGroupDetails_GroupStatus_Id");

            entity.HasOne(d => d.Student).WithMany(p => p.ThesisGroupDetails)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_ThesisGroupDetails_Students_ID");

            entity.HasOne(d => d.StudentThesisGroup).WithMany(p => p.ThesisGroupDetails)
                .HasForeignKey(d => d.StudentThesisGroupId)
                .HasConstraintName("FK_ThesisGroupDetails_ThesisGroups_ID");
        });

        modelBuilder.Entity<ThesisRevision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ThesisRevisions_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.LecturerComment).HasMaxLength(450);
            entity.Property(e => e.Point).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Summary).HasMaxLength(450);
            entity.Property(e => e.ThesisId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Thesis).WithMany(p => p.ThesisRevisions)
                .HasForeignKey(d => d.ThesisId)
                .HasConstraintName("FK_ThesisRevisions_Theses_ID");
        });

        modelBuilder.Entity<ThesisStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ThesisStatus_Id");

            entity.ToTable("ThesisStatus");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<ThesisSupervisor>(entity =>
        {
            entity.HasKey(e => e.ThesisId).HasName("PK_Guide_PK_Thesis_ID");

            entity.Property(e => e.ThesisId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Attitudes).HasColumnType("ntext");
            entity.Property(e => e.Conclusions).HasMaxLength(100);
            entity.Property(e => e.Contents).HasColumnType("ntext");
            entity.Property(e => e.LecturerId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Point).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Results).HasColumnType("ntext");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.ThesisSupervisors)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("FK_ThesisSupervisors_FacultyStaffs_ID");

            entity.HasOne(d => d.Thesis).WithOne(p => p.ThesisSupervisor)
                .HasForeignKey<ThesisSupervisor>(d => d.ThesisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThesisSupervisors_Theses_ID");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Topic_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrainingForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TrainingForms_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrainingLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TrainingLevel_ID");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
