using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.DataAccess.Infrastructure;

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

        [HttpGet]
        public ActionResult Get()
        {
            var users = _unitOfWork.UserResponsitory.GetAll();
            return Ok(users);
        }
    }
}
