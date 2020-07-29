using System.ComponentModel.DataAnnotations;
using CustomerService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.Update {

    public class UpdateIn {

        [Required]
        [BsonRequired]
        [BsonElement("idString")]
        public string idString { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("lastname")]
        public string lastname { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("walletID")]
        public string walletID { get; set; }

        [BsonElement("role")]
        public string role { get; set; }

        [BsonElement("info")]
        public UpdateInfo info { get; set; }

        [BsonElement("settings")]
        public Settings settings { get; set; }
    }
}
