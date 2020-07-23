using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class RegisterObject {

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("lastname")]
        public string lastname { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("passwordHash")]
        public string passwordHash { get; set; }
    }
}
