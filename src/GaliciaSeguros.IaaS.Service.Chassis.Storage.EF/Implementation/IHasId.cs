using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}
