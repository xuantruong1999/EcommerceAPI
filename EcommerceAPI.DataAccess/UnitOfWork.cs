using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.DataAccess.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess
{
    public class UnitOfWork //: IUnitOfWork
    {
        private EcommerceContext _dbContext { get; }
        private bool _disposed = false;

        public UnitOfWork(EcommerceContext DbContext)
        {
            _dbContext = DbContext;
        }

        private IResponsitory<User> userResponsitory;
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
        #endregion getter

        public void SaveChanges()
        {
            CheckIsDisposed();
            _dbContext.SaveChanges();
        }

        private void CheckIsDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposting)
        {
            if (!this._disposed)
            {
                if (disposting)
                {
                    if(_dbContext != null)
                    {
                        _dbContext.Dispose();
                        //_dbContext = null;
                    }
                }
            }
            _disposed = true;
        }
    }
}
