using LexiconLMS.Shared.Dtos;

namespace LexiconLMS.Server.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
