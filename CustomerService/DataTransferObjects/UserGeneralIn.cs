using CustomerService.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class UserGeneralIn {

        [BsonRequired]
        [BsonElement("idString")]
        public string idString { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

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
