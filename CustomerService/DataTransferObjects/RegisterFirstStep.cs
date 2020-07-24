using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class RegisterFirstStep {

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("lastname")]
        public string lastname { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("password")]
        public string password { get; set; }
    }
}
