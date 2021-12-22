using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Contracts;
using GaliciaSeguros.IaaS.Service.Chassis.Storage.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Contracts
{
    public class EFContext : DbContext
    {
        private readonly IEnumerable<IModelBuilderConfiguration> modelBuilderConfigurations;
        private readonly StorageSettings storageSettings;

        public EFContext(
            IEnumerable<IModelBuilderConfiguration> modelBuilderConfigurations, 
            StorageSettings storageSettings)
        {
            this.modelBuilderConfigurations = modelBuilderConfigurations;
            this.storageSettings = storageSettings;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var modelBuilderConfiguration in modelBuilderConfigurations)
            {
                modelBuilderConfiguration.OnCreatingModels(modelBuilder);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(storageSettings.ConnectionString);
            dbContextOptionsBuilder.EnableDetailedErrors();
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(dbContextOptionsBuilder);
        }
    }
}
