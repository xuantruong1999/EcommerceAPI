using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EcommerceAPI.DataAccess.EFModel;
using AutoMapper;

namespace EcommerceWEB.Controllers
{
    [Authorize("Admin")]
    public class HomeController : BaseController
    {
        public HomeController(UserManager<User> userManager, SignInManager<User> signInmanager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork)
            : base(userManager, signInmanager, roleManager, mapper, unitOfWork)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
