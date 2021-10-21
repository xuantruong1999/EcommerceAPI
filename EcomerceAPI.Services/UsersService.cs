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
        Task<Result> CreateUserAsync(UserCreateViewModel model);
        Task<User> GetUserByIDAsync(object id);
        Task<Result> UpdateAsync(UserEditViewModel model);
        Task<Dictionary<string, bool>> GetRolesUserAsync(object id);
        Task<bool> UpdateRolesAsync(string[] roles, User user);
        Task<Result> DeleteAsync(object id);
        Task<IList<string>> GetRoles(User user);
    }
    public class UsersService : BaseService, IUsersService
    {
        protected readonly SignInManager<User> _singInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public UsersService(RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
            _singInManager = signInManager;
            _roleManager = roleManager;
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
        public async Task<Result> CreateUserAsync(UserCreateViewModel model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                var result = await _userManager.CreateAsync(user, model.Password);
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

                var messageCum = "";
                foreach(IdentityError err in result.Errors)
                {
                    messageCum += err.Description + "\n";
                }

                Result error = $"Create user failed with {messageCum}";
                return error;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user or null </returns>
        public async Task<User> GetUserByIDAsync(object id)
        {
            var user = await _userManager.FindByIdAsync((string)id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Get all roles of user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Update User with infors from user input 
        /// </summary>
        /// <returns>Result or null</returns>
        public async Task<Result> UpdateAsync(UserEditViewModel model)
        {
            var user = await GetUserByIDAsync(model.Id);
            if (user == null) return null;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            var result = _userManager.UpdateAsync(user);
            if (result.Result.Succeeded)
            {
                return Result.WithOutErrored;
            }
            else
            {
                Result err = string.Format("Update Fail with username: {0}", model.UserName);
                return err;
            }
        }

        /// <summary>
        /// Get all roles of specifices user to view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dictionary<string, bool></returns>
        public async Task<Dictionary<string,bool>> GetRolesUserAsync(object id)
        {
            var checkUserisExist = await _userManager.FindByIdAsync((string)id);
            if (checkUserisExist != null)
            {
                var roles = _roleManager.Roles.Select(r => r.Name).Distinct().ToList();
                var roleUser = await _userManager.GetRolesAsync(checkUserisExist);
                Dictionary<string, bool> dicRoles = new Dictionary<string, bool>();
                foreach (var item in roles)
                {
                    if (roleUser.Contains(item))
                    {
                        dicRoles.Add(item, true);
                    }
                    else
                    {
                        dicRoles.Add(item, false);
                    }
                }
                return dicRoles;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Update roles's specifices user 
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="user"></param>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateRolesAsync(string[] roles, User user)
        {
            if (roles.Length == 0 || roles == null || user == null) return false;

            var roleUser = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roleUser);
            var result = await _userManager.AddToRolesAsync(user, (IEnumerable<string>)roles);
            if (!result.Succeeded) return false;
            return true;
        }
        
        /// <summary>
        /// Delete user with id
        /// </summary>
        /// <param name="id">id user </param>
        /// <returns>Result</returns>
        public async Task<Result> DeleteAsync(object id)
        {
            var user = await GetUserByIDAsync(id);
            if (user == null)
            {
                Result err = "User is not exist";
                return err;
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Result.WithOutErrored;
            }
            else
            {
                Result err = "Can not delete user with ID: " + id;
                return err;
            }
        }
    }
}
