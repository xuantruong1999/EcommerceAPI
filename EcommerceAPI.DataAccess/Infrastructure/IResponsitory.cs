using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EcommerceAPI.DataAccess.Infrastructure
{
    public interface IResponsitory<T> where T : class
    {
        IEnumerable GetAll();
        T GetByID(object Id);
        int Count { get; }
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object Id);
        void Delete(Expression<Func<T, bool>> predicate);
        void Save();
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable Find(Expression<Func<T, bool>> predicate);

    }
}
