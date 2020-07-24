using System;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class Verify {
        [BsonRequired]
        [BsonElement("address")]
        public Address address { get; set; }

        [BsonRequired]
        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [BsonRequired]
        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [BsonRequired]
        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }

        [BsonElement("region")]
        public string region { get; set; }

        [BsonElement("language")]
        public string language { get; set; }
    }
}
