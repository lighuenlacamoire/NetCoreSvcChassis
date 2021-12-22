using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Implementation;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    public class EFStartup<TDbContext, TModelBuilderConfiguration> : IStorageStartup
        where TDbContext : DbContext
        where TModelBuilderConfiguration : class, IModelBuilderConfiguration
    {
        private readonly IList<(Type DomainType, Type StorageType)> mappings;

        public EFStartup(IList<(Type DomainType, Type StorageType)> mappings)
        {
            this.mappings = mappings;
        }

        /// <inheritdoc />
        public void Configure(IServiceCollection services, StorageSettings storageSettings)
        {
            #region Unit Of Work
            services.AddTransient<IEFUnitOfWork, EFUnitOfWork>();
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
            services.AddScoped<DbContext, TDbContext>();
            services.AddTransient<IModelBuilderConfiguration, TModelBuilderConfiguration>();
            #endregion
        }
    }
}
