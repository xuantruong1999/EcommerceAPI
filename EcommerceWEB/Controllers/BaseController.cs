using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWEB.Controllers
{
    public class BaseController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public BaseController()
        {

        }
        public BaseController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
    }
}
