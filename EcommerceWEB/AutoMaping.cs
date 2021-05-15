using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.Model.User;

namespace EcommerceWEB
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            //UserViewModel to User
            CreateMap<UserViewModel, User>();
            //User to UserViewModel
            CreateMap<User, UserViewModel>();
        }
    }
}
