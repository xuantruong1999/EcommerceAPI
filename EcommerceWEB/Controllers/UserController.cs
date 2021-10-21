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

namespace EcommerceWEB.Controllers
{
    [Authorize]
    [Route("{controller}/{action=index}")]
    public class UserController : BaseController
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUsersService _userService;
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

                var result = await _userService.CreateUser(model);
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

        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    UserEditViewModel userEditView = new UserEditViewModel();
                    userEditView = _mapper.Map<UserEditViewModel>(user);
                    userEditView.Roles = await _userManager.GetRolesAsync(user);
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    userEditView.Claims = userClaims.Select(c => c.Value).ToList();
                    return View(userEditView);
                }
                else
                {
                    ErrorViewModel err = new ErrorViewModel("User is not existed");
                    return View("~/Views/Shared/Error.cshtml", err);
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
                    var user = await _userManager.FindByIdAsync(userModel.Id);
                    if (user == null)
                    {
                        ErrorViewModel err = new ErrorViewModel("User is not existed");
                        return View("~/Views/Shared/Error.cshtml", err);
                    }
                    else
                    {
                        user.Email = userModel.Email;
                        user.UserName = userModel.UserName;
                        user.PhoneNumber = userModel.PhoneNumber;
                        var result = _userManager.UpdateAsync(user);
                        if (result.Result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format("Update Fail with username: {0}", userModel.UserName));
                            return View();
                        }
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
                var checkUserisExist = await _userManager.FindByIdAsync(id);
                if(checkUserisExist != null)
                {
                    var roles =  _roleManager.Roles.Select(r => r.Name).Distinct().ToList();
                    var roleUser = (List<string>) await _userManager.GetRolesAsync(checkUserisExist);
                    Dictionary<string, bool> dicRoles = new Dictionary<string, bool>();
                    foreach(var item in roles)
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
                    ViewData["roles"] = dicRoles;
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
            if (ModelState.IsValid)
            {
                if(userID != null)
                {
                    var user = await _userManager.FindByIdAsync(userID);
                    if(user != null)
                    {
                        if (roles.Length == 0) 
                            return View("Error", new ErrorViewModel("User must have one role in application!"));
                        
                        var roleUser = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, roleUser);
                        var result = await _userManager.AddToRolesAsync(user,(IEnumerable<string>)roles);
                        
                        if (result.Succeeded) 
                        {
                            return RedirectToAction("UpdateRoleUser", new { id = userID});
                        }
                        else 
                        { 
                            return View("Error", new ErrorViewModel($"Update role for user fails"));
                        }
                    }
                    else 
                    {
                        return View("Error", new ErrorViewModel($"User has not existed with id = {userID}"));
                    }
                }
                else
                {
                    return View("Error", new ErrorViewModel($"User has not existed with id = {userID}"));
                }
            }
            else 
            {
                return View("Error", new ErrorViewModel("The params is not valid"));
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            var avatar = _unitOfwork.ProfileResponsitory.GetAll().Where(p => p.UserID.Equals(id)).FirstOrDefault()?.Avatar;
            if(user != null)
            {
                var result = _userManager.DeleteAsync(user); //cascade delete profile
                if (result.Result.Succeeded)
                {
                    DeleteImageExistes(avatar);
                    return RedirectToAction("Index");
                }
                else
                {
                    string message = "Can not delete user with ID: " + id;
                    return View("~/Views/Shared/Error.cshtml", new ErrorViewModel(message));
                }
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel("User is not exist"));
            }
        }

        private void DeleteImageExistes(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            string pathFileToDelete = Path.Combine(_hostingEnvironment.WebRootPath, "Images\\UserImages", fileName);

            if (System.IO.File.Exists(pathFileToDelete))
            {
                System.IO.File.Delete(pathFileToDelete);
            }
        }
    }
}
