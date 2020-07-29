using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.DataTransferObjects.Update {
    public class UpdateInfo {
        [BsonElement("address")]
        public UpdateAddress address { get; set; }

        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }

        [BsonElement("identityNumber")]
        public string identityNumber { get; set; }

        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }

    }
}
