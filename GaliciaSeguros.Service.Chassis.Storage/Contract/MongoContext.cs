using GaliciaSeguros.Service.Chassis.Storage.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GaliciaSeguros.Service.Chassis.Storage.Contract
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(string ConnectionString, string Database)
        {
            var client = new MongoClient(ConnectionString);
            if (client != null)
                _database = client.GetDatabase(Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}