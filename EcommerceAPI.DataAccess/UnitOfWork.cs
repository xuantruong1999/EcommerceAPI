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
        private readonly EcommerceContext _dbContext;
        private IResponsitory<User> userResponsitory;
        private IResponsitory<Profile> profileResponsitory;
        private bool disposedValue = false;
        public UnitOfWork(EcommerceContext DbContext)
        {
            _dbContext = DbContext;
        }
        #region getter
        public IResponsitory<User> UserResponsitory
        {
            get
            {
                if(this.userResponsitory == null)
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
        #endregion getter

        public void SaveChanges()
        {
            CheckIsDisposed();
            _dbContext.SaveChanges();
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

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
