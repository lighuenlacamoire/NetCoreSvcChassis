using GaliciaSeguros.Service.Chassis.Storage.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.Service.Chassis.Storage.Contract
{
    public class MongoStartup<TDbContext, TModelBuilderConfiguration> : IStorageStartup
        where TDbContext : MongoContext
        where TModelBuilderConfiguration : class, IModelBuilderConfiguration
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

                // Type storageInterfaceType = typeof(IDataStorage<>);
                // Type storageConcreteType = typeof(DataStorageEf<>);

                Type constructedRepoInterfaceType = repoInterfaceType.MakeGenericType(domainType);
                Type constructedRepoConcreteType = repoConcreteType.MakeGenericType(domainType, storageType);

                // Type constructedStorageInterfaceType = storageInterfaceType.MakeGenericType(storageType);
                // Type constructedStorageConcreteType = storageConcreteType.MakeGenericType(storageType);

                services.AddTransient(constructedRepoInterfaceType, constructedRepoConcreteType);
                // services.AddTransient(constructedStorageInterfaceType, constructedStorageConcreteType);
            }

            // services.AddScoped<MongoContext, TDbContext>();

            #region DbContext
            services.AddScoped<IMongoContext, MongoContext>(
                provider => new MongoContext(storageSettings.ConnectionString,
                    storageSettings.DbName));
            #endregion
            services.AddTransient<IModelBuilderConfiguration, TModelBuilderConfiguration>();
        }
    }
}
