namespace CustomerService {
    public class Urls {
        public static string NotificationServicePort = "9010";
        public static string EmailServiceURL(string host) {
            return $"https://{host}:{NotificationServicePort}/api/v1/email";
        }
    }
}
