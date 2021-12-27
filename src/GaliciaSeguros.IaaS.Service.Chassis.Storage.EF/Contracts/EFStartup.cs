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
    public class EFStartup<TDbContext> : IStorageStartup
        where TDbContext : DbContext
    {
        
        public EFStartup()
        {
        }

        public void Configure(IServiceCollection services, StorageSettings storageSettings)
        {
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            #region Unit Of Work
            services.AddTransient<IEFUnitOfWork, EFUnitOfWork>();
            #endregion
            
            #region DbContext
            services.AddScoped<DbContext, TDbContext>();
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(storageSettings.ConnectionString);
            });
            #endregion
        }
    }
}
