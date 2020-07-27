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

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(_fromAddress);
            msg.Sender = new MailAddress(_fromAddress);
            msg.To.Add(toAddress);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Credentials = new NetworkCredential(_username, _password);
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(msg);

        }
    }
}
