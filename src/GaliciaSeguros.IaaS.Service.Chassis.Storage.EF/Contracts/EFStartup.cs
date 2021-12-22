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
        private readonly IList<(Type IRepoType, Type RepoType)> mappings;

        public EFStartup(IList<(Type IRepoType, Type RepoType)> mappings)
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

            #region DbContext
            services.AddScoped<DbContext, TDbContext>();
            services.AddTransient<IModelBuilderConfiguration, TModelBuilderConfiguration>();
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(storageSettings.ConnectionString);
            });
            #endregion
        }
    }
}
