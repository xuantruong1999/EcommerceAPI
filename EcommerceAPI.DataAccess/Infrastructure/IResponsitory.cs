using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EcommerceAPI.DataAccess.Infrastructure
{
    public interface IResponsitory<T> where T : class
    {
        IEnumerable GetAll();
        T GetByID();
        int Count { get; }
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object Id);
        void Delete(Expression<Func<T, bool>> predicate);


    }
}
