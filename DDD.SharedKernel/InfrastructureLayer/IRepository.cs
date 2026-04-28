using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DDD.SharedKernel.InfrastructureLayer
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        TEntity Get(long id);
        IList<TEntity> GetAll();
        IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
    }
}
