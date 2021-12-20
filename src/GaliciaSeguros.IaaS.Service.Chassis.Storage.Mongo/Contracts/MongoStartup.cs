using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Contracts
{
    public class MongoStartup : IStorageStartup
    {
        private readonly IList<(Type DomainType, Type StorageType)> mappings;

        public MongoStartup(IList<(Type DomainType, Type StorageType)> mappings)
        {
            this.mappings = mappings;
        }

        /// <inheritdoc />
        public void Configure(IServiceCollection services, StorageSettings storageSettings)
        {
            #region Unit Of Work
            services.AddTransient<IMongoUnitOfWork, MongoUnitOfWork>();
            #endregion
            services.AddSingleton(storageSettings);

            foreach (var (domainType, storageType) in mappings)
            {
                Type repoInterfaceType = typeof(IRepository<>);
                Type repoConcreteType = typeof(BaseRepository<>);

                Type constructedRepoInterfaceType = repoInterfaceType.MakeGenericType(domainType);
                Type constructedRepoConcreteType = repoConcreteType.MakeGenericType(domainType, storageType);

                services.AddTransient(constructedRepoInterfaceType, constructedRepoConcreteType);
            }

            #region DbContext
            services.AddScoped<IMongoContext, MongoContext>(
                provider => new MongoContext(storageSettings.ConnectionString,
                    storageSettings.Database));
            #endregion
        }
    }
}
