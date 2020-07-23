using CustomerService.Contexts;
using CustomerService.Models;

namespace CustomerService.Repositories {
    public class UserRepository : BaseRepository<User>, IUserRepository {
        public UserRepository(IMongoUserDBContext context) : base(context) {

        }
    }
}
