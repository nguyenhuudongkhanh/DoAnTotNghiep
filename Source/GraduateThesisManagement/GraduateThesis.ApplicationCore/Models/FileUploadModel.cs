using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class FileUploadModel
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Length { get; set; }
    public Stream Stream { get; set; }
}
