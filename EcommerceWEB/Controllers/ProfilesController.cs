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
        private readonly ICommonService _commonService;
        protected readonly IWebHostEnvironment _hostingEnviroment;
        public ProfilesController(ICommonService commonService, IMapper mapper, IWebHostEnvironment hostingEnviroment, IProfileService profileService) : base(mapper) 
        {
            _commonService = commonService;
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

                profile.Avatar = "Images/UserImages/" + profile.Avatar;
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
        public async Task<ActionResult> UpdateProfile(ProfileViewModel profile, IFormFile? formFile)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (formFile != null)
                    {
                        string uniqueFilename = await _commonService.UploadProfileImage(formFile);
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
    }
}
