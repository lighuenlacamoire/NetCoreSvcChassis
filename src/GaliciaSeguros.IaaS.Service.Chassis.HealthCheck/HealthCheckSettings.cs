using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.HealthCheck
{
    public class HealthCheckSettings
    {
        public string UIHeader { get; set; }
        public string StorageRegister { get; set; }
        public string UrlPath { get; set; }
        public string SelfPath { get; set; }
        public string UIPath { get; set; }
        public string ApiPath { get; set; }
        public int EvaluationTimeOnSeconds { get; set; }
        public int MinimumSecondsBetweenFailureNotifications { get; set; }
        public int MaximumExecutionHistoriesPerEndpoint { get; set; }
        public string CustomStylesheet { get; set; }
    }
}
