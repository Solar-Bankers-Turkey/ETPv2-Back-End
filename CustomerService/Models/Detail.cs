using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class Detail {

        [BsonElement("address")]
        public Address address { get; set; }

        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }

        [BsonElement("region")]
        public string region { get; set; }

        [BsonElement("language")]
        public string language { get; set; }
    }
}
