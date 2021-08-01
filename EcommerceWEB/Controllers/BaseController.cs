using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EcommerceAPI.DataAccess.EFModel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceWEB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfwork;
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInmanager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public BaseController()
        {
        }
        public BaseController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfwork = unitOfWork;
        }
        public BaseController(UserManager<User> userManager, SignInManager<User> signInmanager, RoleManager<IdentityRole> roleManager,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfwork = unitOfWork;
            _userManager = userManager;
            _signInmanager = signInmanager;
            _roleManager = roleManager;
        }
    }
}
