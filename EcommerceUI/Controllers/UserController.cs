using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceExtention;
using System.Net;

namespace EcommerceUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public ActionResult Get()
        //{
        //   // List<User> users = CommonExtention<User>.ConvertIEnumToList(_unitOfWork.UserResponsitory.GetAll());
        //   // List<Profile> profiles = CommonExtention<Profile>.ConvertIEnumToList(_unitOfWork.ProfileResponsitory.GetAll());
        //   //var results = (from u in users join p in profiles
        //   //               on u.Id equals p.UserID select u.Profile).FirstOrDefault();
        //   // return Ok(results);
        //}
    }
}
