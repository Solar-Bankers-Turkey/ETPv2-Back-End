using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace CustomerService.Repositories {
    public interface IBaseRepository<TEntity> where TEntity : class {
        Task<string> Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        Task<IEnumerable<TEntity>> Query(IQueryCollection query);
        Task<IEnumerable<TEntity>> GetAny(string key, string value);
        Task<IEnumerable<TEntity>> GetAll();
    }
}
