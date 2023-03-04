
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface ISystemRepository
{
    void BackupDatabase();
    List<DbBackupHistoryOutput> GetDbBackupHistory();
    DbBackupHistoryOutput GetBackupHistoryDt(int mediaSetId);
    void ClearDbBackupHistory();
    void RestoreDatabase(int mediaSetId);
}
