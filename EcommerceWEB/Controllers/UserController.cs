using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using EcomerceAPI.Services;
using EcommerceAPI.Services;

namespace EcommerceWEB.Controllers
{
    [Authorize]
    [Route("{controller}/{action=index}")]
    public class UserController : BaseController
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUsersService _userService;
        private readonly ICommonSerivce _commonService;
        public UserController(IUsersService userService, UserManager<User> userManager, SignInManager<User> signInmanager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment) : base(userManager, signInmanager, roleManager, mapper, unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listViewuser = _userService.GetAllUsers();
            return View(listViewuser);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<string> roles = new List<string>();
            roles = _roleManager.Roles.Select(x => x.Name).ToList();
            ViewData["roles"] = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Roles.Any())
                {
                    ModelState.AddModelError("", "Please select role for new user");
                    List<string> allRoles = new List<string>();
                    allRoles = _roleManager.Roles.Select(x => x.Name).ToList();
                    ViewData["roles"] = allRoles;
                    return View(model);
                }
                List<string> roles = new List<string>();
                roles = _roleManager.Roles.Select(x => x.Name).ToList();
                ViewData["roles"] = roles;

                var result = await _userService.CreateUserAsync(model);
                if (result.Errored)
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                foreach(var modelState in ViewData.ModelState.Values)
                {
                    foreach(var error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                return View("Edit");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var user = await _userService.GetUserByIDAsync(id);
                if (user != null)
                {
                    var userView = _mapper.Map<UserEditViewModel>(user);
                    userView.Roles = await _userService.GetRoles(user);
                    return View(userView);
                }
                else
                {
                    ModelState.AddModelError("", "User was not found");
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserEditViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.UpdateAsync(userModel);
                    if (result == null)
                    {
                        ErrorViewModel err = new ErrorViewModel("User is not existed");
                        return View("~/Views/Shared/Error.cshtml", err);
                    }
                    else if (!result.Errored)
                    {
                        return Redirect("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ErrorMessage); 
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ErrorViewModel err = new ErrorViewModel("user model param is not valid");
                return View("~/Views/Shared/Error.cshtml", err);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRoleUser(string id)
        {
            try
            {
                var dicroles = await _userService.GetRolesUserAsync(id);
                if(dicroles != null)
                {
                    ViewData["roles"] = dicroles;
                    ViewData["userID"] = id;
                    return View();
                }
                return View();
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoleUser([FromForm] string[] roles, string userID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userID == null) return View("error", new ErrorViewModel("User ID is not null"));
                    var user = await _userService.GetUserByIDAsync(userID);
                    if (user != null)
                    {
                        if (roles.Length == 0)
                        {
                            ModelState.AddModelError("", "User must have one role in application!");
                            return View();
                        }

                        bool isSuccess = await _userService.UpdateRolesAsync(roles, user);
                        if (isSuccess)
                        {
                            return RedirectToAction("UpdateRoleUser", new { id = userID });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Update roles fails");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"User has not existed with id = {userID}");
                        return View();

                    }

                }
                else
                {
                    ModelState.AddModelError("", "Params are not valid");
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            if(id != null)
            {
                var avatar = _unitOfwork.ProfileResponsitory.GetAll().Where(p => p.UserID.Equals(id)).FirstOrDefault()?.Avatar;
                var result = await _userService.DeleteAsync(id); //cascade delete profile
                if (!result.Errored)
                {
                    _commonService.DeleteImageExistes(avatar);
                    return RedirectToAction("Index");
                }
                else
                {
                    string message = "Can not delete user with ID: " + id;
                    return View("~/Views/Shared/Error.cshtml", result.ErrorMessage);
                }
               
            }
            else
            {
                string message = "Id is not null";
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel(message));
            }
        }
    }
}
