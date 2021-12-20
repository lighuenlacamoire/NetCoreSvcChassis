using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Implementation
{
    public interface IMongoUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}
