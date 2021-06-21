using AutoMapper;
using EcommerceAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWEB.Controllers
{
    public class UserController : BaseController
    {
        protected readonly UserManager<IdentityUser> _userManager;

        protected readonly SignInManager<IdentityUser> _signInmanager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInmanager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _signInmanager = signInmanager;
        }
        [Authorize]
        public IActionResult Index()
        {
            var listUser = _userManager.Users;
            List<UserViewModel> listViewuser = new List<UserViewModel>();

            foreach(var item in listUser)
            {
                UserViewModel user = _mapper.Map<UserViewModel>(item);
                listViewuser.Add(user);
            }

            return View(listViewuser);
        }
    }
}
