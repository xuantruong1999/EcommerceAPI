using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EcommerceUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private EcommerceContext _context { get; set; }
        public UserController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            //var profile = new Profile();

            //var user = new User();
            //user.Email = "Osin@gmail.com";
            //user.Name = "Koman";
            //user.Profile = profile;
            //_context.Database.EnsureCreated();
            //_context.Add(user);
            //_context.SaveChanges();

            var users = from u in _context.Users
                       select u;

            return Ok(users);
        }
    }
}
