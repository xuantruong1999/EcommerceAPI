using AutoMapper;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWEB.Controllers
{
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfwork;
        public BaseAPIController()
        {
        }
        public BaseAPIController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfwork = unitOfWork;
        }
    }
}
