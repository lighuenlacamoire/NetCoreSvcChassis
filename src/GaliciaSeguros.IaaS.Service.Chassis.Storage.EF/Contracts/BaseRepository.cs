using GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    public class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public void Add(TEntity item)
        {
            _dbContext.Set<TEntity>().Add(item);
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(t => true);
        }

        public void Update(TEntity item)
        {
            _dbContext.Set<TEntity>().Attach(item);
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
