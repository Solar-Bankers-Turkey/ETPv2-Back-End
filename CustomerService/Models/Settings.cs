using MongoDB.Bson.Serialization.Attributes;

namespace CustomerService.Models {
    public class Settings {

        [BsonElement("region")]
        public string region { get; set; }

        [BsonElement("language")]
        public string language { get; set; }

        [BsonElement("notification")]
        public Notification notification { get; set; }

    }
}
