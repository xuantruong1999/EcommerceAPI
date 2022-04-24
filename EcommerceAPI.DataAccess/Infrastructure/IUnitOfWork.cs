using EcommerceAPI.DataAccess.EFModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        EcommerceContext dbContext { get; }
        void Save();
        void SaveAsync();
        IRepository<User> UserRepository { get; }
        IRepository<Profile> ProfileRepository { get; }
        IRepository<CategoryProduct> CategoryProductRepository { get; }
        IRepository<Product> ProductRepository { get; }
    }
}
