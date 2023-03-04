using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.File;

public interface IFileManager: IDisposable
{
    void SetPath(string path);
    string GetExtension(string contentType);
    string GetExtension(ExportTypeOptions exportTypeOptions);
    string GetContentType(ExportTypeOptions exportTypeOptions);
    void CreateFolder(string folderName);
    FileStream Read(string path);
    void Save(FileUploadModel uploadModel);
    void Remove(string fileName);
}
