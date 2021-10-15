using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;


namespace EcomerceAPI.Services
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfwork;
        protected readonly UserManager<User> _userManager;
        public BaseService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfwork = unitOfWork;
            _userManager = userManager;
        }
    }
}
