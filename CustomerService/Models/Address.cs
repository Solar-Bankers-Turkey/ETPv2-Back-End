using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class Address {
        [Required]
        [BsonElement("fullAddress")]
        public string fullAddress { get; set; }

        [Required]
        [BsonElement("city")]
        public string city { get; set; }

        [BsonElement("state")]
        public string state { get; set; }

        [Required]
        [BsonElement("country")]
        public string country { get; set; }
    }
}
