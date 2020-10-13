namespace NotificationService.Email {
    public interface IEmailSender {
        void Send(string from, string to, string subject, string body, bool isBodyHtml);
    }
}
