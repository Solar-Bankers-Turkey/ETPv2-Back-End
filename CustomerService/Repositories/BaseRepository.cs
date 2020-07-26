using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Contexts;
using CustomerService.Utils;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomerService.Repositories {

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class {

        private readonly IMongoUserDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;
        private IMongoUserDBContext context;

        protected BaseRepository(IMongoUserDBContext context) {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public async Task<string> Create(TEntity obj) {
            try {
                await _dbCollection.InsertOneAsync(obj);
            } catch (Exception e) {
                throw e;
            }
            return Utils.RepositoryUtils.getVal(obj, "Id");
        }

        public void Delete(string id) {
            var objectId = new ObjectId(id);
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }

        public async Task<IEnumerable<TEntity>> Query(IQueryCollection query) {
            if (!RepositoryUtils.isQueryValid<TEntity>(query)) {
                throw new Utils.NotValidQueryException("not a valid query");
            }
            var builder = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Empty;
            int count = 0;
            foreach (var item in query) {
                if (count == 0) {
                    filter = builder.Eq(item.Key, item.Value);
                } else {
                    filter = filter & builder.Eq(item.Key, item.Value);
                }
                count++;
            }
            var ent = await _dbCollection.FindAsync(filter).Result.ToListAsync();
            return ent;
        }

        public async Task<IEnumerable<TEntity>> GetAny(string key, string value) {
            if (key == "id") {
                var objectId = new ObjectId(value);
                return await _dbCollection.FindAsync(Builders<TEntity>.Filter.Eq("_id", objectId)).Result.ToListAsync();
            }
            var builder = Builders<TEntity>.Filter;
            var filter = builder.Eq(key, value);
            var ent = await _dbCollection.FindAsync(filter).Result.ToListAsync();
            return ent;
        }

        public async Task<IEnumerable<TEntity>> GetAll() {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        public async void Update(TEntity obj) {
            await _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetType().GetProperty("Id").GetValue(obj, null)), obj);
        }

    }
}
