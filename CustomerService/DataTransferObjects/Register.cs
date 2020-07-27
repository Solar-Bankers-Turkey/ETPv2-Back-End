using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class Register {
        [Required]
        [BsonRequired]
        [BsonElement("name")]
        public string name { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("lastname")]
        public string lastname { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("email")]
        public string email { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("password")]
        public string password { get; set; }
    }
}
