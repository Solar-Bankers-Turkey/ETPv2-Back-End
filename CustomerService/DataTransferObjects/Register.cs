using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class Register {
        [BsonRequired]
        [BsonElement("name")]
        public string name { get; set; }

        [BsonRequired]
        [BsonElement("lastname")]
        public string lastname { get; set; }

        [BsonRequired]
        [BsonElement("email")]
        public string email { get; set; }

        [BsonRequired]
        [BsonElement("password")]
        public string password { get; set; }
    }
}
