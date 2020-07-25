using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace CustomerService.Repositories {
    public interface IBaseRepository<TEntity> where TEntity : class {
        Task<string> Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        public Task<IEnumerable<TEntity>> Query(IQueryCollection query);
        public Task<TEntity> GetAny(string key, string value);
        // Task<List<TEntity>> Get(FilterDefinition<TEntity> filter);
        Task<IEnumerable<TEntity>> Get();
    }
}
