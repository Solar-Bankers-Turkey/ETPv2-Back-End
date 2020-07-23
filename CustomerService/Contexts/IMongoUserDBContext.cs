using MongoDB.Driver;

namespace CustomerService.Model {
    public interface IMongoUserDBContext {
        IMongoCollection<User> GetCollection<User>(string name);
    }
}
