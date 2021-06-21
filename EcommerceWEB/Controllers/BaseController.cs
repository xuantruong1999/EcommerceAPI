using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWEB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMapper _mapper;

        public BaseController()
        {
        }
        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
