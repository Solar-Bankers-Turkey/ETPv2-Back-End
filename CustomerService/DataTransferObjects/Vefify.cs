using System;
using System.ComponentModel.DataAnnotations;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects {
    public class Verify {

        [Required]
        [BsonRequired]
        [BsonElement("address")]
        public Address address { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }

        [BsonElement("registrationDate")]
        public DateTime registrationDate { get; set; }

        [BsonElement("region")]
        public string region { get; set; }

        [BsonElement("language")]
        public string language { get; set; }
    }
}
