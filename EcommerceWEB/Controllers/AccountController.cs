using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceExtention;
using EcommerceAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using EcommerceAPI.DataAccess;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EcommerceAPI.DataAccess.EFModel;
using AutoMapper;

namespace EcommerceWEB.Controllers
{
    
    public class AccountController : BaseController
    {
        public AccountController(UserManager<User> userManager, SignInManager<User> signInmanager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork)
           : base(userManager, signInmanager, roleManager, mapper, unitOfWork)
        {
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new UserLoginViewModel { ReturnURL = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string emailPattern = string.Format($"^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
                bool validateEmail = Regex.IsMatch(model.UserNameOrEmail, emailPattern);
                string returnURL = model.ReturnURL ?? Url.Content("~/user/index");
                if (!validateEmail)
                {
                    var result = await _signInmanager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if(Url.IsLocalUrl(returnURL))
                            return Redirect("~/");
                        return Redirect(Url.Content("~/"));
                    }
                    else
                    {
                        ModelState.AddModelError("", string.Format($"Login Fail with UserName: {model.UserNameOrEmail} and password"));
                        return View(model);
                    }
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                    if (user != null)
                    {
                        var result = await _signInmanager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return LocalRedirect(returnURL);
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format($"Login Fail with email: {model.UserNameOrEmail} and password"));
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", string.Format($"User have not existed with email: {model.UserNameOrEmail} and password"));
                        return View(model);
                    }
                }
            }   
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    var roleCheck = await _roleManager.RoleExistsAsync("Member");
                    if (!roleCheck)
                    {
                        IdentityRole role = new IdentityRole()
                        {
                            Name = "Member"
                        };

                        var statusCreaterole = await _roleManager.CreateAsync(role);

                        if (statusCreaterole.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Member");
                        }

                    }
                    await _userManager.AddToRoleAsync(user, "Member");
                    await _signInmanager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach( var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInmanager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
    }
}
