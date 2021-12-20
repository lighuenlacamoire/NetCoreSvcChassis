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
            var swaggerSettings = GetSwaggerSettings(services, configuration);
            if (swaggerSettings != null)
            {
                this.serviceName = swaggerSettings.serviceName;
                this.serviceDescription = swaggerSettings.serviceDescription;
                this.serviceVersion = swaggerSettings.serviceVersion;

                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = swaggerSettings.serviceName,
                            Description = swaggerSettings.serviceDescription,
                            Version = swaggerSettings.serviceVersion,
                            Contact = new OpenApiContact
                            {
                                Name = "Galicia Seguros",
                                Email = string.Empty,
                                Url = new Uri("https://x.com"),
                            },
                        });
                    options.EnableAnnotations();
                    options.CustomSchemaIds(type => Regex.Replace(type.ToString(), @"[^a-zA-Z0-9._-]+", ""));
                    // XML Documentation
                    try
                    {
                        var xmlFile = "ServiceDocumentation.xml";
                        // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        // var xmlPath = Path.Combine(webHostEnvironment.ContentRootPath, xmlFile);
                        if (File.Exists(xmlPath))
                            options.IncludeXmlComments(xmlPath);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }
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
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerPrefix == string.Empty
                                ? "/swagger/v1/swagger.json"
                                : $"/{swaggerPrefix}/swagger/v1/swagger.json", $"{serviceName} {serviceVersion}");
            });
            #endregion
        }
        /// <summary>
        /// Obtiene la configuracion para Swagger
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static SwaggerSettings GetSwaggerSettings(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<SwaggerSettings>(configuration.GetSection("SwaggerSettings"));

                var storageSettings = serviceCollection.BuildServiceProvider().GetService<IOptions<SwaggerSettings>>();
                return storageSettings.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
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
