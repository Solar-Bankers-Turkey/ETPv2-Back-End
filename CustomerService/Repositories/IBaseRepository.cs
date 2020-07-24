using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace CustomerService.Repositories {
    public interface IBaseRepository<TEntity> where TEntity : class {
        void Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        public Task<TEntity> GetFromID(string id);
        public Task<TEntity> Query(IQueryCollection queries);
        public Task<TEntity> GetAny(string key, string value);
        // Task<List<TEntity>> Get(FilterDefinition<TEntity> filter);
        Task<IEnumerable<TEntity>> Get();
    }
}
