using System.Net;
using System.Net.Mail;

namespace GraduateThesis.ApplicationCore.Email;

public class SmtpService : IEmailService
{
    private SmtpClient _smtp;
    private SmtpConfiguration _smtpConfiguration;
    private bool disposedValue;

    public SmtpService(SmtpConfiguration smtpConfiguration)
    {
        _smtp = new SmtpClient();
        _smtp.UseDefaultCredentials = false;

        _smtp.Host = smtpConfiguration.Host;
        _smtp.Port = smtpConfiguration.Port;
        _smtp.EnableSsl = smtpConfiguration.EnableSsl;
        _smtp.Credentials = new NetworkCredential(smtpConfiguration.User, smtpConfiguration.Password);

        _smtpConfiguration = smtpConfiguration;
    }

    public void Send(string recipient, string subject, string content)
    {
        if (!_smtpConfiguration.Enable)
            return;

        MailAddress from = new MailAddress(_smtpConfiguration.Address, _smtpConfiguration.DisplayName);
        MailAddress to = new MailAddress(recipient);

        MailMessage mailMessage = new MailMessage(from, to);
        mailMessage.Subject = subject;
        mailMessage.Body = content;

        mailMessage.IsBodyHtml = true;

        _smtp.Send(mailMessage);
    }

    public void Send(string[] recipients, string subject, string content)
    {
        if (!_smtpConfiguration.Enable)
            return;

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_smtpConfiguration.Address, _smtpConfiguration.DisplayName);

        foreach(string recipient in recipients)
        {
            mailMessage.To.Add(new MailAddress(recipient));
        }

        mailMessage.Subject = subject;
        mailMessage.Body = content;

        mailMessage.IsBodyHtml = true;

        _smtp.Send(mailMessage);
    }

    public async Task SendAsync(string recipient, string subject, string content)
    {
        if (!_smtpConfiguration.Enable)
            return;

        MailAddress from = new MailAddress(_smtpConfiguration.Address, _smtpConfiguration.DisplayName);
        MailAddress to = new MailAddress(recipient);

        MailMessage mailMessage = new MailMessage(from, to);
        mailMessage.Subject = subject;
        mailMessage.Body = content;

        mailMessage.IsBodyHtml = true;

        await _smtp.SendMailAsync(mailMessage);
    }

    public async Task SendAsync(string[] recipients, string subject, string content)
    {
        if (!_smtpConfiguration.Enable)
            return;

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_smtpConfiguration.Address, _smtpConfiguration.DisplayName);

        foreach (string recipient in recipients)
        {
            mailMessage.To.Add(new MailAddress(recipient));
        }

        mailMessage.Subject = subject;
        mailMessage.Body = content;

        mailMessage.IsBodyHtml = true;

        await _smtp.SendMailAsync(mailMessage);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            _smtp.Dispose();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
