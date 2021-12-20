using GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Implementation;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Contracts
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private IEnumerable<TEntity> Data = null;
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(
            IMongoContext context,
            string endpoint = null)
        {
            Context = context;
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual IEnumerable<TEntity> JsonToClass(string content)
        {
            return JsonConvert.DeserializeObject<List<TEntity>>(content);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return DbSet.AsQueryable().ToList();
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsQueryable().Where(predicate).ToList();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
