using MongoDB.Driver;

namespace CustomerService.Contexts {
    public interface IMongoUserDBContext {
        IMongoCollection<User> GetCollection<User>(string name);
    }
}
