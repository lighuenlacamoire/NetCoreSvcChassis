using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts
{
    /// <summary>
    /// Configuration del tipo de Storage
    /// </summary>
    public class StorageSettings
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Nomenclatura de la base de datos
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Tipo de Base de datos
        /// </summary>
        public DatabaseType DbType { get; set; }

    }

    public enum DatabaseType
    {
        MongoDb,
        SqlServer,
        InMemory
    }
}
