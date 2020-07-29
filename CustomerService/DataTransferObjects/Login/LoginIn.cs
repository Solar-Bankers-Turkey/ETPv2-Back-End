using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.Login {
    public class LoginIn {

        [Required]
        [BsonRequired]
        [BsonElement("email")]
        public string email { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("password")]
        public string password { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("rememberMe")]
        public bool rememberMe { get; set; }
    }
}
