using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWEB.Controllers
{
    public class UserController : BaseController
    {   protected readonly IUnitOfWork _unitofwork;
        public UserController(IUnitOfWork _unitofwork) : base(_unitofwork)
        {
        }
        public IActionResult Index()
        {
            _unitOfWork.UserResponsitory.GetAll();
            return View();
        }
    }
}
