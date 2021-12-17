using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.Service.Chassis.Storage.Implementation
{
    public interface IMongoUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}
