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
using EcommerceAPI.Services;

namespace EcommerceWEB.Controllers
{
    [Authorize("Admin")]
    public class ProfilesController : BaseController
    {
        private readonly IProfileService _profileService;
        protected readonly IHostingEnvironment _hostingEnviroment;
        public ProfilesController(IMapper mapper, IHostingEnvironment hostingEnviroment, IProfileService profileService) : base(mapper) 
        {
            _profileService = profileService;
            _hostingEnviroment = hostingEnviroment;
        }
        
        [HttpGet]
        public ActionResult ViewProfile()
        {
            try
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var profile = _profileService.GetProfileByName(currentUser.Identity.Name);
                if (profile != null)
                {
                    var profileToView = _mapper.Map<ProfileViewModel>(profile);
                    return View(profileToView);
                }
                else
                {
                    return View("Error", new ErrorViewModel("User is not exist"));
                }
            }
            catch(Exception ex)
            {
                throw ex;
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
                        string uploadFolder = Path.Combine(_hostingEnviroment.WebRootPath, "Images\\UserImages");
                        string uniqueFilename = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFilename);
                        using (FileStream avar = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(avar);
                        }
                            
                        profile.Avatar = uniqueFilename;
                    }

                    // Checking user in Database. Has user existed before?
                    var prf = _mapper.Map<EcommerceAPI.DataAccess.EFModel.Profile>(profile);
                    var result = _profileService.Update(prf);
                    if (!result) 
                    {
                        ModelState.AddModelError("", "Update profile fails");
                        return View("ViewProFile", profile);
                    }

                    return RedirectToAction("ViewProfile");
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
            string pathFileToDelete = Path.Combine(_hostingEnviroment.WebRootPath, "Images\\UserImages", fileName);

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
