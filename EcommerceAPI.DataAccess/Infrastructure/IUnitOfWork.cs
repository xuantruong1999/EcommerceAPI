using EcommerceAPI.DataAccess.EFModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        EcommerceContext DbContext { get; }
        void SaveChanges();
        IResponsitory<User> UserResponsitory { get;}
        IResponsitory<Profile> ProfileResponsitory { get;}
    }
}
