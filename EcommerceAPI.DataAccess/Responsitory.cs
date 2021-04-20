using EcommerceAPI.DataAccess.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EcommerceAPI.DataAccess.Respository
{
    public class Responsitory<T> : IResponsitory<T> where T : class
    {
        public int Count => throw new NotImplementedException();

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetByID()
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
