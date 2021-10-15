using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcomerceAPI.Services
{
    interface IUsersService
    {
        IList<UserViewModel> GetAllUsers();
        //async Task<IActionResult> Create(UserCreateViewModel model);
    }
    public class UsersService : BaseService, IUsersService
    {
        protected readonly SignInManager<User> _singInManager;
        public UsersService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager) : base(unitOfWork, userManager)
        {
            _singInManager = signInManager;
        }

        public IList<UserViewModel> GetAllUsers()
        {
            var listUser = _userManager.Users.Where(u => u.UserName != _singInManager.Context.User.Identity.Name);
            List<UserViewModel> listViewuser = new List<UserViewModel>();

            foreach (var item in listUser)
            {
                UserViewModel user = _mapper.Map<UserViewModel>(item);
                listViewuser.Add(user);
            }

            return listViewuser;
        }
    }
}
