using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace NotificationService.Email {
    public class EmailSender : IEmailSender {
        private string _smtpServer;
        private int _smtpPort;
        private string _username;
        private string _password;
        private bool _enableSsl;
        private bool _useDefaultCredentials;

        public EmailSender(IConfiguration configuration) {
            _smtpServer = configuration["VerificationMail:SmtpServer"];
            _smtpPort = int.Parse(configuration["VerificationMail:SmtpPort"]);
            _username = configuration["VerificationMail:SmtpUsername"];
            _password = configuration["VerificationMail:SmtpPassword"];
            _enableSsl = bool.Parse(configuration["VerificationMail:EnableSsl"]);
            _useDefaultCredentials = bool.Parse(configuration["VerificationMail:UseDefaultCredentials"]);
        }

        public async void Send(string _from, string _to, string _subject, string _body, bool _isBodyHtml) {
            using(MailMessage mail = new MailMessage(_from, _to, _subject, _body)) {
                mail.Sender = new MailAddress(_from);
                mail.IsBodyHtml = _isBodyHtml;
                using(SmtpClient client = new SmtpClient()) {
                    client.EnableSsl = _enableSsl;
                    client.UseDefaultCredentials = _useDefaultCredentials;
                    client.Host = _smtpServer;
                    client.Credentials = new NetworkCredential(_username, _password);
                    client.Port = _smtpPort;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await client.SendMailAsync(mail);
                }
            }
        }
    }
}
