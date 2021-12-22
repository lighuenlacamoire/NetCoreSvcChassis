using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddCustomizedDatabase(this IServiceCollection services, IConfiguration configuration, IStorageStartup storageStartup)
        {

            var storageSettings = GetSettings(services, configuration);
            if (storageStartup != null && storageSettings != null)
            {
                storageStartup.Configure(services, storageSettings);
            }

             return services;
        }

        /// <summary>
        /// Obtiene la configuracion para base de datos
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static StorageSettings GetSettings(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<StorageSettings>(configuration.GetSection("SQLSettings"));

                var storageSettings = serviceCollection.BuildServiceProvider().GetService<IOptions<StorageSettings>>();
                return storageSettings.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
