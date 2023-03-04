using GraduateThesis.ApplicationCore.Models;
using Microsoft.AspNetCore.Http;

namespace GraduateThesis.WebExtensions;

public static class FormFileExtensions
{
    public static FileUploadModel ToFileUploadModel(this IFormFile formFile)
    {
        return new FileUploadModel
        {
            FileName = formFile.FileName,
            ContentType = formFile.ContentType,
            Length = formFile.Length,
            Stream = formFile.OpenReadStream()
        };
    }
}
