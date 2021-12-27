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
        protected readonly DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbContext.Set<TEntity>());
        }

        public BaseRepository(DbContext context)
        {
            _dbContext = context;
        }
        public void Add(TEntity item)
        {
            _dbContext.Set<TEntity>().Add(item);
            // DbSet.Add(item);
        }

        public void Delete(TEntity item)
        {
            // var existingEntity = _dbContext.Set<TEntity>().FirstOrDefault(t => t.Id == item.Id);
            DbSet.Remove(item);
            // _dbContext.Set<TEntity>().Remove(existingEntity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(t => true);
        }

        public void Update(TEntity item)
        {
            // var existingEntity = dataStorage.GetOne(item.Id);
            // mapper.Map(item, existingEntity);

            _dbContext.Set<TEntity>().Attach(item);
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
