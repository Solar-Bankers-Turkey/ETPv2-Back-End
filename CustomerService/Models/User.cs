using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [Required]
        [BsonElement("passwordHash")]
        public string passwordHash { get; set; }

        [Required]
        [BsonElement("name")]
        public string name { get; set; }

        [Required]
        [BsonElement("lastname")]
        public string lastname { get; set; }

        [Required]
        [BsonElement("email")]
        public string email { get; set; }

        // [Required]
        [BsonElement("walletID")]
        public string walletID { get; set; }

        [Required]
        [BsonElement("customerType")]
        public string customerType { get; set; }

        [Required]
        [BsonElement("verified")]
        public bool verified { get; set; }

        [Required]
        [BsonElement("detail")]
        public Detail detail { get; set; }
    }
}
