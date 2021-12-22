using GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(DbContext context)
        {
            dbContext = context;
            DbSet = dbContext.Set<TEntity>();
        }
        public void Add(TEntity item)
        {
            DbSet.Add(item);
        }

        public void Delete(TEntity item)
        {
            // var existingEntity = dbContext.Set<TEntity>().FirstOrDefault(t => t.Id == item.Id);
            DbSet.Remove(item);
            // dbContext.Set<TEntity>().Remove(existingEntity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().Where(t => true);
        }

        public void Update(TEntity item)
        {
            // var existingEntity = dataStorage.GetOne(item.Id);
            // mapper.Map(item, existingEntity);

            dbContext.Set<TEntity>().Attach(item);
            dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
