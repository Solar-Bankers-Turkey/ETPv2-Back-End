using System.ComponentModel.DataAnnotations;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.General {
    public class GeneralOut {

        [Required]
        [BsonRequired]
        [BsonElement("idString")]
        public string idString { get; set; }

        [Required]
        [BsonElement("name")]
        public string name { get; set; }

        [Required]
        [BsonElement("lastname")]
        public string lastname { get; set; }

        [Required]
        [BsonElement("email")]
        public string email { get; set; }

        [Required]
        [BsonElement("walletID")]
        public string walletID { get; set; }

        [Required]
        [BsonElement("customerType")]
        public string customerType { get; set; }

        [Required]
        [BsonElement("role")]
        public string role { get; set; }

        [Required]
        [BsonElement("info")]
        public Info info { get; set; }

        [Required]
        [BsonElement("settings")]
        public Settings settings { get; set; }
    }
}
