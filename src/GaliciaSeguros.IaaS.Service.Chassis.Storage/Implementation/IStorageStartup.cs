using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Implementation
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
