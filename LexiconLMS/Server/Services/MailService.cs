using LexiconLMS.Shared.Dtos;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LexiconLMS.Server.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(MailData mailData)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailSettings.SenderEmail);
            mailMessage.To.Add(mailData.EmailTo);
            mailMessage.Subject = mailData.EmailSubject;
            mailMessage.Body = mailData.EmailBody;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = _mailSettings.Host;
            smtpClient.Port = _mailSettings.Port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
