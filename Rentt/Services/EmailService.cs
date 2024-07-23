using System.Net.Mail;
using System.Net;

namespace Rentt.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<string> _logger;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _enableSsl;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _toEmail;

        public EmailService(
            ILogger<string> logger, 
            IConfiguration configuration)
        {
            _logger = logger;
            _smtpServer = configuration["EmailSettings:SmtpServer"] ?? string.Empty;
            _smtpPort = Convert.ToInt32(configuration["EmailSettings:SmtpPort"]);
            _enableSsl = Convert.ToBoolean(configuration["EmailSettings:EnableSsl"]);
            _senderEmail = configuration["EmailSettings:SenderEmail"] ?? string.Empty;
            _senderPassword = configuration["EmailSettings:SenderPassword"] ?? string.Empty;
            _toEmail = configuration["EmailSettings:ToEmail"] ?? string.Empty;
        }

        public async Task Send(string subject, string body)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_senderEmail, _senderPassword)
            };

            var message = new MailMessage(_senderEmail, _toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                _logger.LogInformation("E-mail enviado com sucesso. De: {sender} Para: {email}", _senderEmail, _toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao enviar e-mail. De: {sender} Para: {email} Exception: {exception}", _senderEmail, _toEmail, ex);
            }
        }
    }
}
