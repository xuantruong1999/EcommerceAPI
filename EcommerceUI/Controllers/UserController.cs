using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        //public UserController()
        //{

        //}
        [HttpGet]
        public ActionResult Get()
        {
           Dictionary<string, string>  start = new Dictionary<string, string>()
            {
               
            };

            start.Add("1", "Nguyen Van A");
            start.Add("2", "Nguyen Van B");
            start.Add("3", "Nguyen Van C");
            return Ok(start);
        }
    }
}
