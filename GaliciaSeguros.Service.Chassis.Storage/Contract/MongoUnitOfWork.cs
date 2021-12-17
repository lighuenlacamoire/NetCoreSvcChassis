using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaliciaSeguros.Service.Chassis.Storage.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace GaliciaSeguros.Service.Chassis.Storage.Contract
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {

        private readonly IServiceScope serviceScope;
        private readonly IMongoContext dbContext;
        private bool disposedValue;

        public MongoUnitOfWork(IServiceProvider serviceProvider)
        {
            serviceScope = serviceProvider.CreateScope();
            dbContext = serviceScope.ServiceProvider.GetRequiredService<IMongoContext>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return serviceScope.ServiceProvider.GetService<IRepository<TEntity>>();
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
