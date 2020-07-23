using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("userName")]
        public string userName { get; set; }

        [BsonElement("passwordHash")]
        public string passwordHash { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("lastname")]
        public string lastname { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("walletID")]
        public string walletID { get; set; }

        [BsonElement("customerType")]
        public string customerType { get; set; }

        [BsonElement("detail")]
        public Detail detail { get; set; }
    }
}
