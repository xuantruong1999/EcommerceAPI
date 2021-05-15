using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWEB.Controllers
{
    public class UserController : BaseController
    {   protected readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork _unitofwork, IMapper mapper) : base(_unitofwork)
        {
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var a = _unitOfWork.UserResponsitory.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = _mapper.Map<User>(userViewModel);
                _unitofwork.UserResponsitory.Insert(newUser);
                _unitofwork.UserResponsitory.Save();

            }
        }
    }
}
