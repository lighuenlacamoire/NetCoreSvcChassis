using GaliciaSeguros.Service.Chassis.Storage.Implementation;
using GaliciaSeguros.Service.Chassis.Storage.Contract;
using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GaliciaSeguros.Service.Chassis.API.Configuration
{
    /// <summary>
    /// Configura los CCC
    /// </summary>
    public class ServiceStartup
    {
        private readonly string serviceName;
        private readonly string serviceDescription;
        private readonly string serviceVersion;
        private readonly bool isDevelopment;
        private readonly IStorageStartup storageStartup;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private const string swaggerPrefix = "";

        public ServiceStartup(
            string serviceName,
            string serviceDescription,
            string serviceVersion,
            bool isDevelopment, 
            IStorageStartup storageStartup, 
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment)
        {
            this.serviceName = serviceName;
            this.serviceDescription = serviceDescription;
            this.serviceVersion = serviceVersion;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = serviceName,
                        Description = serviceDescription,
                        Version = serviceVersion,
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
            #endregion

            var storageSettings = GetStorageSettings(services, configuration);
            
            if (storageStartup != null && storageSettings != null)
                storageStartup.Configure(services, storageSettings);
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
        /// Obtiene la configuracion para base de datos
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static StorageSettings GetStorageSettings(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<StorageSettings>(configuration.GetSection("StorageSettings"));

                var storageSettings = serviceCollection.BuildServiceProvider().GetService<IOptions<StorageSettings>>();
                return storageSettings.Value;
            }
            catch
            {
                return null;
            }
        }

    }
}
