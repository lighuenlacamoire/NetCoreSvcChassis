using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Implementation;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Mongo.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Swagger;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Implementation;

namespace GaliciaSeguros.IaaS.Service.Chassis.API.Contracts
{
    /// <summary>
    /// Configura los CCC
    /// </summary>
    public class ServiceStartup
    {
        private string serviceName = "";
        private string serviceDescription = "";
        private string serviceVersion = "";
        private readonly bool isDevelopment;
        private readonly IStorageStartup storageStartup;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private const string swaggerPrefix = "";

        public ServiceStartup(
            bool isDevelopment,
            IStorageStartup storageStartup,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            this.isDevelopment = isDevelopment;
            this.storageStartup = storageStartup;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Configura la inyeccion de dependencias
        /// </summary>
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            #region Swagger
            #endregion

            #region Base de Datos: Mongo
            var mongoSettings = GetMongoSettings(services, configuration);

            if (storageStartup != null && mongoSettings != null)
                storageStartup.Configure(services, mongoSettings);
            #endregion
        }

        /// <summary>
        /// Configura la aplicacion
        /// </summary>
        public void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            #region Swagger
            #endregion
        }
        
        /// <summary>
        /// Obtiene la configuracion para base de datos
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static StorageSettings GetMongoSettings(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<StorageSettings>(configuration.GetSection("MongoSettings"));

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
