using AutoMapper;
using EcommerceAPI.DataAccess;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;


namespace EcommerceAPI.Services
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfwork;
        protected readonly UserManager<User> _userManager;
        protected readonly EcommerceContext _dbContext;
        public BaseService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfwork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
    }
}
