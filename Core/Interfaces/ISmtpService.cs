using Core.Models.Smtp;

namespace Core.Interfaces;

public interface ISmtpService
{
    Task<bool> SendEmailAsync(EmailMessage message);
}
