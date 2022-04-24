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
        private EcommerceContext dbContext;
        private IRepository<User> userRepository;
        private IRepository<Profile> profileRepository;
        private IRepository<CategoryProduct> categoryProduct;
        private IRepository<Product> product;
        private bool disposedValue = false;
        public UnitOfWork(EcommerceContext DbContext)
        {
            dbContext = DbContext;
        }

        #region getter
        EcommerceContext IUnitOfWork.dbContext => this.dbContext;

        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<User>(dbContext);
                }
                return this.userRepository;
            }
        }

        public IRepository<Profile> ProfileRepository
        {
            get
            {
                if (this.profileRepository == null)
                {
                    this.profileRepository = new Repository<Profile>(dbContext);
                }
                return this.profileRepository;
            }
        }

        public IRepository<CategoryProduct> CategoryProductRepository
        {
            get
            {
                if (this.categoryProduct == null)
                {
                    this.categoryProduct = new Repository<CategoryProduct>(dbContext);
                }
                return this.categoryProduct;
            }
        }

        public IRepository<Product> ProductRepository
        {
            get
            {
                if (this.product == null)
                {
                    this.product = new Repository<Product>(dbContext);
                }
                return this.product;
            }
        }

       
        #endregion getter

        public void Save()
        {
            CheckIsDisposed();
            dbContext.SaveChanges();
        }
        public void SaveAsync()
        {
            dbContext.SaveChangesAsync();
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
                    if(dbContext != null)
                    {
                        dbContext.Dispose();
                        

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
