using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess
{
    public class UnitOfWork 
    {
        private EcommerceContext _dbContext { get; }
        public UnitOfWork()
        {
            _dbContext = new EcommerceContext();
        }
    }
}
