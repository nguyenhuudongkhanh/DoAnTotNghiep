using GraduateThesis.ApplicationCore.Enums;

namespace GraduateThesis.ApplicationCore.File;

public class FileType
{
    public string GetExtension(string contentType)
    {
        switch (contentType)
        {
            case ContentTypeConsts.JPEG:
                return nameof(ContentTypeConsts.JPEG).ToLower();
            case ContentTypeConsts.PNG:
                return nameof(ContentTypeConsts.PNG).ToLower();
            case ContentTypeConsts.GIF:
                return nameof(ContentTypeConsts.GIF).ToLower();
            case ContentTypeConsts.BMP:
                return nameof(ContentTypeConsts.BMP).ToLower();
            case ContentTypeConsts.DOC:
                return nameof(ContentTypeConsts.DOC).ToLower();
            case ContentTypeConsts.DOCX:
                return nameof(ContentTypeConsts.DOCX).ToLower();
            case ContentTypeConsts.PPT:
                return nameof(ContentTypeConsts.PPT).ToLower();
            case ContentTypeConsts.PPTX:
                return nameof(ContentTypeConsts.PPTX).ToLower();
            case ContentTypeConsts.XLS:
                return nameof(ContentTypeConsts.XLS).ToLower();
            case ContentTypeConsts.XLSX:
                return nameof(ContentTypeConsts.XLSX).ToLower();
            case ContentTypeConsts.RAR:
                return nameof(ContentTypeConsts.RAR).ToLower();
            case ContentTypeConsts.ZIP:
                return nameof(ContentTypeConsts.ZIP).ToLower();
            case ContentTypeConsts.MP3:
                return nameof(ContentTypeConsts.MP3).ToLower();
            case ContentTypeConsts.M4A:
                return nameof(ContentTypeConsts.M4A).ToLower();
            case ContentTypeConsts.MP4:
                return nameof(ContentTypeConsts.MP4).ToLower();
            case ContentTypeConsts.AVI:
                return nameof(ContentTypeConsts.AVI).ToLower();
            default:
                throw new Exception("'content-type' does not match");
        }
    }

    public string GetExtension(ExportTypeOptions exportTypeOptions)
    {
        return exportTypeOptions.ToString().ToLower();
    }

    public string GetContentType(ExportTypeOptions exportTypeOptions)
    {
        switch (exportTypeOptions)
        {
            case ExportTypeOptions.XLS:
                return ContentTypeConsts.XLS;
            case ExportTypeOptions.XLSX:
                return ContentTypeConsts.XLSX;
            default:
                return ContentTypeConsts.XLS;
        }
    }
}
