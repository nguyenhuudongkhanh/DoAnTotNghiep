using GraduateThesis.ApplicationCore.AppDatabase;
using GraduateThesis.ApplicationCore.AppSettings;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace GraduateThesis.ApplicationCore.Context;

public partial class AppDbContext : DbContext
{
    public static string ConnectionString = null;

    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {

    }

    public static AppDbContext CreateContext()
    {
        DbContextOptionsBuilder<AppDbContext> dbCtxOptBulder = new DbContextOptionsBuilder<AppDbContext>();
        dbCtxOptBulder.UseSqlServer(AppDefaultValue.ConnectionString);

        return new AppDbContext(dbCtxOptBulder.Options);
    }

    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<AppRoleMapping> AppRoleMappings { get; set; }
    public DbSet<AppPage> AppPages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserRoles_AppRoles_ID ");
        });
    }
}
