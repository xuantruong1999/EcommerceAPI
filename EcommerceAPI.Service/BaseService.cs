using EcommerceAPI.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.Service
{
    public class BaseService 
    {
        public IUnitOfWork _unitOfwork;
        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }
    }
}
