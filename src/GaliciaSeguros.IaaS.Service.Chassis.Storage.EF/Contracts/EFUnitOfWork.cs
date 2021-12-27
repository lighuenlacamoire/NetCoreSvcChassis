using GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    internal class EFUnitOfWork : IEFUnitOfWork
    {
        private readonly IServiceScope serviceScope;
        private readonly DbContext dbContext;
        private bool disposedValue;

        public EFUnitOfWork(IServiceProvider serviceProvider)
        {
            serviceScope = serviceProvider.CreateScope();
            dbContext = serviceScope.ServiceProvider.GetRequiredService<DbContext>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return serviceScope.ServiceProvider.GetService<IRepository<TEntity>>();
        }

        public int Commit()
        {
            return dbContext.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    serviceScope.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
