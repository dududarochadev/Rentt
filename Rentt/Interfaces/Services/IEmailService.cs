namespace Rentt.Services
{
    public interface IEmailService
    {
        Task Send(string subject, string body);
    }
}
