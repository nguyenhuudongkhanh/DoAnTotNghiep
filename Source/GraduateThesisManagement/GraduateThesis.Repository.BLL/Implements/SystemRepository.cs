using GraduateThesis.ApplicationCore.AppSettings;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraduateThesis.Repository.BLL.Implements;

public class SystemRepository : ISystemRepository
{
    private HufiGraduateThesisContext _context;

    internal SystemRepository(HufiGraduateThesisContext context)
    {
        _context = context;
    }

    public void BackupDatabase()
    {
        string backupFilePath = null;
        if (string.IsNullOrEmpty(AppDefaultValue.DbBackupFilePath))
            backupFilePath = $"{AppDefaultValue.DbName}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.bak";
        else
            backupFilePath = $"{AppDefaultValue.DbBackupFilePath}\\{AppDefaultValue.DbName}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.bak";
        
        DateTime expirateDate = DateTime.Now.AddDays(30);
        _context.Database.ExecuteSqlRaw(
            "BACKUP DATABASE {0} TO DISK = {1} WITH EXPIREDATE = {2}", 
            AppDefaultValue.DbName, 
            backupFilePath, 
            expirateDate
        );
    }

    public List<DbBackupHistoryOutput> GetDbBackupHistory()
    {
        string query = @"
            SELECT
               CONVERT(CHAR(100), SERVERPROPERTY('Servername')) AS[Server], 
               msdb.dbo.backupset.[database_name] AS DatabaseName,
               msdb.dbo.backupset.backup_start_date AS StartDate, 
               msdb.dbo.backupset.backup_finish_date AS FinishDate, 
               msdb.dbo.backupset.expiration_date AS ExpirationDate, 
               CASE msdb..backupset.type
                  WHEN 'D' THEN 'Database'
                  WHEN 'L' THEN 'Log'
                  END AS BackupType, 
               msdb.dbo.backupset.backup_size AS BackupSize, 
               msdb.dbo.backupmediafamily.logical_device_name AS LogicalDeviceName, 
               msdb.dbo.backupmediafamily.physical_device_name AS PhysicalDeviceName, 
               msdb.dbo.backupmediafamily.media_set_id AS MediaSetId,
               msdb.dbo.backupset.name AS BackupSetName, 
               msdb.dbo.backupset.[description] AS[Description]
            FROM
               msdb.dbo.backupmediafamily
               INNER JOIN msdb.dbo.backupset ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id
            WHERE
               (CONVERT(datetime, msdb.dbo.backupset.backup_start_date, 102) >= GETDATE() - 7)  AND msdb.dbo.backupset.[database_name] = {0}
            ORDER BY
               msdb.dbo.backupset.[database_name], 
               msdb.dbo.backupset.backup_finish_date";

        return _context.DbBackupHistories
            .FromSqlRaw(query, AppDefaultValue.DbName)
            .ToList().OrderByDescending(d => d.StartDate).ToList();
    }

    public DbBackupHistoryOutput GetBackupHistoryDt(int mediaSetId)
    {
        string query = @"
            SELECT
               CONVERT(CHAR(100), SERVERPROPERTY('Servername')) AS[Server], 
               msdb.dbo.backupset.[database_name] AS DatabaseName,
               msdb.dbo.backupset.backup_start_date AS StartDate, 
               msdb.dbo.backupset.backup_finish_date AS FinishDate, 
               msdb.dbo.backupset.expiration_date AS ExpirationDate, 
               CASE msdb..backupset.type
                  WHEN 'D' THEN 'Database'
                  WHEN 'L' THEN 'Log'
                  END AS BackupType, 
               msdb.dbo.backupset.backup_size AS BackupSize, 
               msdb.dbo.backupmediafamily.logical_device_name AS LogicalDeviceName, 
               msdb.dbo.backupmediafamily.physical_device_name AS PhysicalDeviceName, 
               msdb.dbo.backupmediafamily.media_set_id AS MediaSetId,
               msdb.dbo.backupset.name AS BackupSetName, 
               msdb.dbo.backupset.[description] AS[Description]
            FROM
               msdb.dbo.backupmediafamily
               INNER JOIN msdb.dbo.backupset ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id
            WHERE
               (CONVERT(datetime, msdb.dbo.backupset.backup_start_date, 102) >= GETDATE() - 7)  
               AND msdb.dbo.backupset.[database_name] = {0}
               AND msdb.dbo.backupset.media_set_id = {1}
            ORDER BY
               msdb.dbo.backupset.[database_name], 
               msdb.dbo.backupset.backup_finish_date";

        return _context.DbBackupHistories
            .FromSqlRaw(query, AppDefaultValue.DbName, mediaSetId)
            .ToList().OrderByDescending(d => d.StartDate).SingleOrDefault();
    }

    public void RestoreDatabase(int mediaSetId)
    {
        string physicalDeviceNameQuery = @"
            SELECT physical_device_name AS [Value]
            FROM msdb.dbo.backupmediafamily
            WHERE media_set_id = {0}";

        string physicalDeviceName = _context.Database
            .SqlQueryRaw<string>(physicalDeviceNameQuery, mediaSetId).SingleOrDefault();

        string killConnectQuery = @"
            USE [master]

            DECLARE @kill varchar(8000) = '';  
            SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
            FROM sys.dm_exec_sessions
            WHERE database_id  = db_id({0})

            EXEC(@kill)";

        _context.Database.ExecuteSqlRaw(killConnectQuery, AppDefaultValue.DbName);

        string restoreQuery = @"
            USE [master]
            RESTORE DATABASE {0} FROM DISK = {1} WITH REPLACE";

        _context.Database.ExecuteSqlRaw(
            restoreQuery,
            AppDefaultValue.DbName,
            physicalDeviceName
        );
    }

    public void ClearDbBackupHistory()
    {
        _context.Database.ExecuteSqlRaw(
            "EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'HUFI_graduate-thesis'",
            AppDefaultValue.DbName
        );
    }
}
