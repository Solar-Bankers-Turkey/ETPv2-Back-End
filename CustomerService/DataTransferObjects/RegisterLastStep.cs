using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class RegisterLastStep {

        [BsonElement("walletID")]
        public string walletID { get; set; }

        [BsonElement("customerType")]
        public string customerType { get; set; }

        [BsonElement("detail")]
        public Detail detail { get; set; }
    }
}
