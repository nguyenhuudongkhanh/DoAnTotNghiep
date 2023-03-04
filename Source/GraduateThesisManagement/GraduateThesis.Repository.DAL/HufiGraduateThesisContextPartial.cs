using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.Repository.DAL;

public partial class HufiGraduateThesisContext : DbContext
{
    public DbSet<DbBackupHistoryOutput> DbBackupHistories { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbBackupHistoryOutput>().HasNoKey();
    }
}
