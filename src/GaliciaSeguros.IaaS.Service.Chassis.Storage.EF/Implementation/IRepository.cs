using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GaliciaSeguros.IaaS.Service.Chassis.Storage.EF.Implementation
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);

        void Delete(TEntity item);

        void Update(TEntity item);

        IEnumerable<TEntity> GetAll();
    }
}
