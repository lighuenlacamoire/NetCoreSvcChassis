using GaliciaSeguros.Service.Chassis.Storage.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.Service.Chassis.Storage.Implementation
{
    public interface IStorageStartup
    {
        /// <summary>
        /// Configura la injeccion de dependencias para la configuracion de la base
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storageSettings"></param>
        void Configure(IServiceCollection services, StorageSettings storageSettings);
    }
}
