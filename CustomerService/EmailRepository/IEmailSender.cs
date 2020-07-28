namespace CustomerService.Email {
    public interface IEmailSender {
        void Send(string toAddress, string subject, string body);
    }
}
