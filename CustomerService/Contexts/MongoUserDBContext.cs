﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomerService.Contexts {
    public class MongoUserDBContext : IMongoUserDBContext {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoUserDBContext(IOptions<MongoSettings> configuration) {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) {
            name = name.ToLower();
            return _db.GetCollection<T>(name);
        }
    }
}
