using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

namespace GaliciaSegurosSvcChassis.Configuration
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
        private const string swaggerPrefix = "";

        public ServiceStartup(string serviceName, string serviceDescription, string serviceVersion, bool isDevelopment)
        {
            this.serviceName = serviceName;
            this.serviceDescription = serviceDescription;
            this.serviceVersion = serviceVersion;
            this.isDevelopment = isDevelopment;
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
                            Name = "Banco Galicia",
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
        }

        /// <summary>
        /// Configura la aplicacion
        /// </summary>
        public void ConfigureApplication(IApplicationBuilder app)
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
    }
}
