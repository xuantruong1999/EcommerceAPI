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
using EcommerceAPI.UI.Controllers;
using EcommerceAPI.DataAccess.EFModel;

namespace EcommerceAPI.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<User> users = _unitOfWork.UserResponsitory.GetAll();

            return Ok(users.ToList());
        }

        [HttpGet("{Id}")]
        public ActionResult Details([FromQuery] string Id)
        {
            if (Id == null) return BadRequest("");
            var convertID = Guid.Parse(Id);
            User user = _unitOfWork.UserResponsitory.GetByID(convertID);

            return Ok(user);
        }
    }
}
