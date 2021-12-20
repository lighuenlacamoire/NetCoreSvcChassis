using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Implementation;
using MongoDB.Driver;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Contracts
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
