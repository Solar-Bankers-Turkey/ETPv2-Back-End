using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.Registration {
    public class RegisterOut {

        [Required]
        [BsonRequired]
        [BsonElement("idString")]
        public string idString { get; set; }

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

    }
}
