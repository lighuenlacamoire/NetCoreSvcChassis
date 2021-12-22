using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace GaliciaSeguros.IaaS.Service.Chassis.Swagger.Contracts
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddCustomizedSwagger(
            this IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment env)
        {

            var swaggerSettings = GetSettings(services, configuration);
            if (swaggerSettings != null)
            {
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

            return services;
        }

        public static IApplicationBuilder UseCustomizedSwagger(
            this IApplicationBuilder app, IWebHostEnvironment env)
        {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", $"ServiceDocumentation");
                });

            return app;
        }
        /// <summary>
        /// Obtiene la configuracion para Swagger
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static SwaggerSettings GetSettings(IServiceCollection serviceCollection, IConfiguration configuration)
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
    }
}
