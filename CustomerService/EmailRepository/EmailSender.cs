using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Email {
    public class EmailSender : IEmailSender {
        private string _smtpServer;
        private int _smtpPort;
        private string _fromAddress;
        private string _fromAddressTitle;
        private string _username;
        private string _password;
        private bool _enableSsl;
        private bool _useDefaultCredentials;

        public EmailSender(IConfiguration configuration) {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpPort = _smtpPort == 0 ? 465 : _smtpPort;
            _fromAddress = configuration["Email:FromAddress"];
            _fromAddressTitle = configuration["FromAddressTitle"];
            _username = configuration["Email:SmtpUsername"];
            _password = configuration["Email:SmtpPassword"];
            _enableSsl = bool.Parse(configuration["Email:EnableSsl"]);
            _useDefaultCredentials = bool.Parse(configuration["Email:UseDefaultCredentials"]);
        }

        public async void Send(string toAddress, string subject, string body, bool sendAsync = true) {

            using(MailMessage mail = new MailMessage()) {
                mail.From = new MailAddress(_fromAddress);
                mail.Sender = new MailAddress(_fromAddress);
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                using(SmtpClient client = new SmtpClient()) {
                    client.EnableSsl = _enableSsl;
                    client.UseDefaultCredentials = _useDefaultCredentials;
                    client.Host = _smtpServer;
                    client.Credentials = new NetworkCredential(_username, _password);
                    client.Port = _smtpPort;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail);
                }
            }

        }
    }
}
