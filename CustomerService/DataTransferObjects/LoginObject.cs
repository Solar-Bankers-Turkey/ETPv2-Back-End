using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class LoginObject {

        [Required]
        [BsonRequired]
        [BsonElement("idString")]
        public string idString { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("password")]
        public string password { get; set; }

        [BsonElement("rememberMe")]
        public bool rememberMe { get; set; }
    }
}
