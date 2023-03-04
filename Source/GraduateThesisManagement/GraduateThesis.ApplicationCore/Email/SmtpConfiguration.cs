#nullable disable

namespace GraduateThesis.ApplicationCore.Email;

public class SmtpConfiguration
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }

    public string Address { get; set; }
    public string DisplayName { get; set; }

    public bool Enable { get; set; }
}
