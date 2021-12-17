using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GaliciaSeguros.Service.Chassis.Storage.Implementation
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> JsonToClass(string content);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate);
    }
}
