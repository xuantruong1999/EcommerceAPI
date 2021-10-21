using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomerceAPI.Services
{
    public interface IUsersService
    {
        IList<UserViewModel> GetAllUsers();
        Task<Result> CreateUser(UserCreateViewModel model);
    }
    public class UsersService : BaseService, IUsersService
    {
        protected readonly SignInManager<User> _singInManager;
        public UsersService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
            _singInManager = signInManager;
        }

        /// <summary>
        /// Get all user from table User 
        /// </summary>
        /// <returns>A list user view model</returns>
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


        /// <summary>
        /// Create a user with informations provided and then adding roles for the one
        /// </summary>
        /// <param name="model">UserCreateViewModel model</param>
        /// <returns>Result</returns>
        public async Task<Result> CreateUser(UserCreateViewModel model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                var result =  await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var addRoletoUser = await _userManager.AddToRolesAsync(user, (IEnumerable<string>)model.Roles);
                    if (addRoletoUser.Succeeded)
                    {
                        return Result.WithOutErrored;
                    }
                    else
                    {
                        Result errorResult = "Add roles failed";
                        return errorResult;
                    }
                }

                Result error = "Create user failed";
                return error;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
