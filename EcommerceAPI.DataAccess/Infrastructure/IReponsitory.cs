﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EcommerceAPI.DataAccess.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T GetByID(object Id);
        int Count { get; }
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object Id);
        void Delete(Expression<Func<T, bool>> predicate);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

    }
}
