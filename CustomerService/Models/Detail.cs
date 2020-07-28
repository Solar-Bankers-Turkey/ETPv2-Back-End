using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class Detail {

        [Required]
        [BsonElement("address")]
        public Address address { get; set; }

        [Required]
        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [Required]
        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [Required]
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
