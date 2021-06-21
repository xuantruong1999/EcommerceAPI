using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Service.System;
using EcommerceWEB.Models;
using System;
using System.Threading.Tasks;

namespace EcommerceAPI.Service
{
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        //public IEquatable<User> GetAll()
        //{
        //    _unitOfWork.
        //}

    }
}
