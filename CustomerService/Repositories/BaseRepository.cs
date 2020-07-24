using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Contexts;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomerService.Repositories {

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class {

        private readonly IMongoUserDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected BaseRepository(IMongoUserDBContext context) {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public async void Create(TEntity obj) {
            // ! null control
            if (obj == null) {
                throw new ArgumentNullException();
            }
            try {
                await _dbCollection.InsertOneAsync(obj);
                var result = await GetAny("email", obj.GetType().GetProperty("email").GetValue(obj, null).ToString());
                if (result == null) {

                }
                var id = obj.GetType().GetProperty("Id").GetValue(obj, null);

            } catch (Exception e) {
                throw e;
            }
        }

        public void Delete(string id) {
            var objectId = new ObjectId(id);
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }

        public async Task<TEntity> GetFromID(string id) {
            var builder = Builders<TEntity>.Filter;
            var filter = builder.Eq("_id", id);
            var ent = await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return ent;
        }

        public async Task<TEntity> Query(IQueryCollection queries) {
            var builder = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter = builder.Eq("0", "0");
            int count = 0;
            foreach (var item in queries) {
                if (count == 0) {
                    filter = builder.Eq(item.Key, item.Value);

                } else {
                    filter = filter & builder.Eq(item.Key, item.Value);

                }
                count++;
            }
            var ent = await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return ent;
        }

        public async Task<TEntity> GetAny(string key, string value) {
            var builder = Builders<TEntity>.Filter;
            var filter = builder.Eq(key, value);
            var ent = await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return ent;
        }

        public async Task<IEnumerable<TEntity>> Get() {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        // public async Task<List<TEntity>> Get(FilterDefinition<TEntity> filter) {
        //     var result = await _dbCollection.FindAsync<TEntity>(filter);
        //     return await result.ToListAsync();
        // }

        public async void Update(TEntity obj) {
            await _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetType().GetProperty("Id").GetValue(obj, null)), obj);
        }

    }
}
