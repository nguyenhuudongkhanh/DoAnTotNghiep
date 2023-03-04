
namespace GraduateThesis.ApplicationCore.Email;

public interface IEmailService : IDisposable
{
    void Send(string recipient, string subject, string content);
    void Send(string[] recipients, string subject, string content);
    Task SendAsync(string recipient, string subject, string content);
    Task SendAsync(string[] recipients, string subject, string content);
}
