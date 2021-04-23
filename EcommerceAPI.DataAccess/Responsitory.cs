using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EcommerceAPI.DataAccess.Respository
{
    public class Responsitory<T> : IResponsitory<T> where T : class
    {
        protected readonly EcommerceContext _dbContext;
        private DbSet<T> _dbSet;
        private EcommerceContext dbContext;

        public Responsitory(IUnitOfWork unitOfWork)
        {
            _dbContext = unitOfWork.DbContext;
            _dbSet = _dbContext.Set<T>();

        }

        public Responsitory(EcommerceContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int Count
        {
            get { return _dbSet.Count(); } 
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(object Id)
        {
            T entity = GetByID(Id);
            Delete(entity);
        }

        public IQueryable Find(Expression<Func<T, bool>> predicate)
        {
           return _dbSet.Where(predicate);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            IQueryable listEntity = Filter(predicate);
            foreach(T entity in listEntity)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IEnumerable GetAll()
        {
            return _dbSet;
        }

        public T GetByID(object Id)
        {
            T entity = _dbSet.Find(Id);
            return entity;
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        //save

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
