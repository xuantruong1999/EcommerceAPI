using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.DataAccess.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private EcommerceContext _dbContext;
        private IResponsitory<User> userResponsitory;
        private IResponsitory<Profile> profileResponsitory;
        private IResponsitory<CategoryProduct> categoryProduct;
        private IResponsitory<Product> product;
        private bool disposedValue = false;
        public UnitOfWork(EcommerceContext DbContext)
        {
            _dbContext = DbContext;
        }
        
        public EcommerceContext DbContext 
        {
            get
            {
                if(_dbContext == null)
                {
                    _dbContext = new EcommerceContext();
                }
                return _dbContext;
            }
        }
        #region getter
        public IResponsitory<User> UserResponsitory
        {
            get
            {
                if (this.userResponsitory == null)
                {
                    this.userResponsitory = new Responsitory<User>(_dbContext);
                }
                return this.userResponsitory;
            }
        }

        public IResponsitory<Profile> ProfileResponsitory
        {
            get
            {
                if (this.profileResponsitory == null)
                {
                    this.profileResponsitory = new Responsitory<Profile>(_dbContext);
                }
                return this.profileResponsitory;
            }
        }

        public IResponsitory<CategoryProduct> CategoryProductResponsitory
        {
            get
            {
                if (this.categoryProduct == null)
                {
                    this.categoryProduct = new Responsitory<CategoryProduct>(_dbContext);
                }
                return this.categoryProduct;
            }
        }

        public IResponsitory<Product> ProductResponsitory
        {
            get
            {
                if (this.product == null)
                {
                    this.product = new Responsitory<Product>(_dbContext);
                }
                return this.product;
            }
        }
        #endregion getter

        public void Save()
        {
            CheckIsDisposed();
            _dbContext.SaveChanges();
        }
        public void SaveAsync()
        {
            _dbContext.SaveChangesAsync();
        }

        private void CheckIsDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if(_dbContext != null)
                    {
                        _dbContext.Dispose();
                        

                    } 
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
