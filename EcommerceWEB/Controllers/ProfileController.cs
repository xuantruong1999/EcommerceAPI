using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Model.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EcommerceAPI.DataAccess.EFModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using EcommerceAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWEB.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;
        public ProfileController(IHostingEnvironment hostingEnvironment, UserManager<User> userManager, SignInManager<User> signInmanager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfwork) 
            : base(userManager, signInmanager, roleManager, mapper, unitOfwork)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        [HttpGet]
        public ActionResult ViewProfile()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userExist = _unitOfwork.UserResponsitory.Find(u => u.UserName == currentUser.Identity.Name).FirstOrDefault();
            
            if (userExist != null)
            {
                var profileByuserID = _unitOfwork.ProfileResponsitory.Find(profile => profile.UserID == userExist.Id).FirstOrDefault();
                ProfileViewModel profileToView = new ProfileViewModel();
                if (profileByuserID == null)
                {
                    profileToView.UserID = userExist.Id;
                    return View(profileToView);
                }
                else
                {
                    profileToView = _mapper.Map<ProfileViewModel>(profileByuserID);
                    return View(profileToView);
                }
                
            }
            else
            {
                return View("Error", new ErrorViewModel("User is not exist"));
            }
        }
        
        [HttpPost]
        public  ActionResult UpdateProfile(ProfileViewModel profile, IFormFile? formFile)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (formFile != null)
                    {
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images\\UserImages");
                        string uniqueFilename = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFilename);
                        using (FileStream avar = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(avar);
                        }
                            
                        profile.Avatar = uniqueFilename;
                    }

                    // Checking user in Database. Has user existed before?
                    var profileInDB = _unitOfwork.ProfileResponsitory.Find(p => p.UserID == profile.UserID)?.FirstOrDefault();
                       
                    if (profileInDB != null)
                    {
                        //Update profile
                        var oldImage = profileInDB.Avatar;
                        profileInDB.FirstName = profile.FirstName;
                        profileInDB.LastName = profile.LastName;
                        profileInDB.Address = profile.Address;
                        
                        if(profile.Avatar != null && profileInDB.Avatar != profile.Avatar)
                            profileInDB.Avatar = profile.Avatar;
                        
                        _unitOfwork.ProfileResponsitory.Update(profileInDB);
                        _unitOfwork.Save();

                        if (!string.IsNullOrEmpty(profile.Avatar)
                            && !string.IsNullOrEmpty(oldImage)
                            && profile.Avatar != oldImage)
                        {
                            DeleteImageExisted(oldImage);
                        }

                        return View("ViewProFile", _mapper.Map<ProfileViewModel>(profileInDB));
                    }
                    else
                    {
                        //Insert Profile
                        EcommerceAPI.DataAccess.EFModel.Profile profileToupdate = new EcommerceAPI.DataAccess.EFModel.Profile()
                        {
                            FirstName = profile.FirstName,
                            LastName = profile.LastName,
                            Address = profile.Address,
                            Avatar = profile.Avatar,
                            UserID = profile.UserID
                        };

                        if (profile.Avatar != null) profileToupdate.Avatar = profile.Avatar;
                        _unitOfwork.ProfileResponsitory.Insert(profileToupdate);
                        _unitOfwork.Save();
                        return View("ViewProFile", _mapper.Map<ProfileViewModel>(profileToupdate));
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Parrams is invalid"));
            }
        }

        private bool DeleteImageExisted(string fileName)
        {
            string pathFileToDelete = Path.Combine(_hostingEnvironment.WebRootPath, "Images\\UserImages", fileName);

            if (System.IO.File.Exists(pathFileToDelete))
            {
                System.IO.File.Delete(pathFileToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
