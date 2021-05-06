using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceWEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWEB.Controllers
{
    public class UserController : BaseController
    {   
        protected readonly IUnitOfWork _unitofwork;
        public UserController(IUnitOfWork _unitofwork) : base(_unitofwork)
        {
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel user)
        {

            if (user == null)
            {
                ErrorViewModel error = new ErrorViewModel("Information login User is not null");
                return Redirect("/error");
            }
            return Redirect("/home");
        }
    }
}
