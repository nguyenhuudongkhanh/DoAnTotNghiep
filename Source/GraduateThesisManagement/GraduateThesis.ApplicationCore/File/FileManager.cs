using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;

#nullable disable

namespace GraduateThesis.ApplicationCore.File;

public class FileManager : IFileManager
{
    private FileType _fileType;
    private string _path;
    private bool disposedValue;

    public FileManager()
    {
        _fileType = new FileType();
    }

    public void Remove(string fileName)
    {
        string filePath = $"{_path}\\{fileName}";
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        else
            throw new Exception("File does not exist!");
    }

    public void Save(Stream stream, string fileName)
    {
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream($"{_path}\\{fileName}", FileMode.Create);
            stream.CopyTo(fileStream);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving the file!", ex);
        }
        finally
        {
            if (fileStream != null)
                fileStream.Dispose();
        }
    }

    public string GetExtension(string contentType)
    {
        return _fileType.GetExtension(contentType);
    }

    public string GetExtension(ExportTypeOptions exportTypeOptions)
    {
        return _fileType.GetExtension(exportTypeOptions);
    }

    public string GetContentType(ExportTypeOptions exportTypeOptions)
    {
        return _fileType.GetContentType(exportTypeOptions);
    }

    public void SetPath(string path)
    {
        _path = path;
    }

    public void CreateFolder(string folderName)
    {
        Directory.CreateDirectory($"{_path}\\{folderName}");
    }

    public FileStream Read(string path)
    {
        return new FileStream(path, FileMode.Open);
    }

    public void Save(FileUploadModel uploadModel)
    {
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream($"{_path}\\{uploadModel.FileName}", FileMode.Create);
            uploadModel.Stream.CopyTo(fileStream);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving the file!", ex);
        }
        finally
        {
            if (uploadModel.Stream != null)
                uploadModel.Stream.Dispose();

            if (fileStream != null)
                fileStream.Dispose();
        }
    }

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