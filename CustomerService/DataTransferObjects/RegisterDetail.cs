using System;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class RegisterDetail {

        [BsonElement("address")]
        public Address address { get; set; }

        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }
    }
}
