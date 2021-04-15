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
        private EcommerceEntity _context { get; set; }
        public UserController(EcommerceEntity context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var profile = new Profile();
            
            var user = new User();
            user.Email = "kadinxuantruong@gmail.com";
            user.Name = "Truong";
            user.Profile = profile;
            
            _context.Users.Add(user);
            var track = _context.SaveChanges();



            return Ok(user);
        }
    }
}
