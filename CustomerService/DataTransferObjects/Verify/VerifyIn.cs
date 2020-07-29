using System.ComponentModel.DataAnnotations;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.Verify {
    public class VerifyIn {

        [Required]
        [BsonElement("Info")]
        public Info info { get; set; }

        [BsonElement("settings")]
        public Settings settings { get; set; }
    }
}
