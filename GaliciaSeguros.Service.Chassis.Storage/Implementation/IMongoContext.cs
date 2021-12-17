using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GaliciaSeguros.Service.Chassis.Storage.Implementation
{
    public interface IMongoContext : IDisposable
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
