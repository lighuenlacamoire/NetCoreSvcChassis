using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecks.UI;
using HealthChecks.UI.Client;
using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace GaliciaSeguros.IaaS.Service.Chassis.HealthCheck.Contracts
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddCustomizedHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration,
        string? connectionString)
        {
            var healthCheckSettings = GetSettings(services, configuration);

            if (healthCheckSettings != null)
            {
                var healthChecks = services.AddHealthChecks();

                healthChecks
                    .AddCheck<ApiHealthCheck>("ApiService", failureStatus: HealthStatus.Unhealthy);
                if (!string.IsNullOrEmpty(connectionString))
                {
                    healthChecks
                        .AddSqlServer(
                            connectionString,
                            healthQuery: "select 1",
                            failureStatus: HealthStatus.Degraded,
                            name: "Database",
                            tags: new string[] { "services" });

                }
                /* 
                services
                    .AddHealthChecks()
                    .AddCheck<ApiHealthCheck>("ApiService", failureStatus: HealthStatus.Unhealthy)
                    .AddSqlServer(
                            connectionString,
                            healthQuery: "select 1",
                            failureStatus: HealthStatus.Degraded,
                            name: "Database",
                            tags: new string[] { "services" });
                
                */

                services
                    .AddHealthChecksUI(setupSettings: setup =>
                    {
                        setup.SetHeaderText(healthCheckSettings.UIHeader);
                        setup.AddHealthCheckEndpoint("ApiService", healthCheckSettings.UrlPath); //map health check api
                        setup.SetEvaluationTimeInSeconds(healthCheckSettings.EvaluationTimeOnSeconds);
                        setup.SetMinimumSecondsBetweenFailureNotifications(healthCheckSettings.MinimumSecondsBetweenFailureNotifications);
                        setup.MaximumHistoryEntriesPerEndpoint(healthCheckSettings.MaximumExecutionHistoriesPerEndpoint);
                    }).AddSqlServerStorage(healthCheckSettings.StorageRegister);
            }


            return services;
        }

        public static WebApplication UseCustomizedHealthCheck(
            this WebApplication app)
        {
            var healthCheckSettings = GetSettings(app);

            if (healthCheckSettings != null)
            {
                app.MapHealthChecks(healthCheckSettings.UrlPath, new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = WriteResponse
                });
                app.MapHealthChecks(healthCheckSettings.SelfPath, new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("ApiService")
                });

                app.MapHealthChecksUI(setup =>
                {
                    setup.AddCustomStylesheet(healthCheckSettings.CustomStylesheet);
                    setup.UIPath = healthCheckSettings.UIPath;
                    setup.ApiPath = healthCheckSettings.ApiPath;
                });
            }
            return app;
        }

        private static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions { Indented = true };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteStartObject("results");

                foreach (var healthReportEntry in healthReport.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key);
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description",
                        healthReportEntry.Value.Description);
                    jsonWriter.WriteStartObject("data");

                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);

                        JsonSerializer.Serialize(jsonWriter, item.Value,
                            item.Value?.GetType() ?? typeof(object));
                    }

                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(
                Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
        /// <summary>
        /// Obtiene la configuracion para los healthcheck
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static HealthCheckSettings GetSettings(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<HealthCheckSettings>(configuration.GetSection("HealthCheckSettings"));

                var settings = serviceCollection.BuildServiceProvider().GetService<IOptions<HealthCheckSettings>>();
                return settings.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private static HealthCheckSettings GetSettings(IApplicationBuilder app)
        {
            try
            {
                var settings = app.ApplicationServices.GetService<IOptions<HealthCheckSettings>>();

                return settings.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
