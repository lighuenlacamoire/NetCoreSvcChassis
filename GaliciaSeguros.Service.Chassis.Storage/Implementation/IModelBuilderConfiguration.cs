using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.Service.Chassis.Storage.Implementation
{
    public interface IModelBuilderConfiguration
    {
        void OnCreatingModels(ModelBuilder builder);
    }
}
