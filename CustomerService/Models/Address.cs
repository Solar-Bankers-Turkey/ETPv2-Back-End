using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class Address {
        [BsonElement("fullAddress")]
        public string fullAddress { get; set; }

        [BsonElement("city")]
        public string city { get; set; }

        [BsonElement("state")]
        public string state { get; set; }

        [BsonElement("country")]
        public string country { get; set; }
    }
}
