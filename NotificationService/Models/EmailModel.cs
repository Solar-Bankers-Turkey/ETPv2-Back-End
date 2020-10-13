using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models {
    public class EmailModel {
        [Required]
        public string subject { get; set; }

        [Required]
        public string to { get; set; }

        [Required]
        public string body { get; set; }

        [Required]
        public bool isBodyHtml { get; set; }
    }
}
