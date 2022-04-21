using AutoMapper;
using EcommerceAPI.DataAccess;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcommerceAPI.Services
{
    public interface IProfileService
    {
        DataAccess.EFModel.Profile GetProfileByName(string userName);
        bool CreateProfile(DataAccess.EFModel.Profile profile);
        bool Update(DataAccess.EFModel.Profile profile);
    }
    public class ProfileService : BaseService, IProfileService
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        protected readonly IBlobStorageAccountService _blobStorageService;
        
        public ProfileService(IBlobStorageAccountService blobStorage, IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _blobStorageService = blobStorage;
        }

        /// <summary>
        /// Get profile by user name input 
        /// </summary>
        /// <param name="userName">current user name </param>
        /// <returns>return new profile || profile existed</returns>
        public DataAccess.EFModel.Profile GetProfileByName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            //var userExist = _unitOfwork.UserResponsitory.Find(u => u.UserName == userName).FirstOrDefault();
            var userExist = _unitOfwork.dbContext.Users.Where(u => u.UserName == userName).Include(u => u.Profile).FirstOrDefault();
            if (userExist != null)
            {
                var profileByuserID = _unitOfwork.ProfileResponsitory.Find(profile => profile.UserID == userExist.Id).FirstOrDefault();
                if (profileByuserID == null)
                {
                    DataAccess.EFModel.Profile profile = new DataAccess.EFModel.Profile();
                    profile.UserID = userExist.Id;
                    return profile;
                }
                else
                {
                    return profileByuserID;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a new Profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool CreateProfile(DataAccess.EFModel.Profile profile)
        {
            try
            {
                if (profile == null)
                    throw new ArgumentNullException();

                _unitOfwork.ProfileResponsitory.Insert(profile);
                _unitOfwork.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(DataAccess.EFModel.Profile profile)
        {
            try
            {
                if (profile == null)
                    return false;

                var profileInDB = _unitOfwork.ProfileResponsitory.Find(p => p.UserID == profile.UserID)?.FirstOrDefault();
                if (profileInDB != null)
                {
                    var oldImage = profileInDB.Avatar;
                    profileInDB.FirstName = profile.FirstName;
                    profileInDB.LastName = profile.LastName;
                    profileInDB.Address = profile.Address;

                    if (profile.Avatar != null && profileInDB.Avatar != profile.Avatar)
                    {
                        profileInDB.Avatar = profile.Avatar;
                    }

                    _unitOfwork.ProfileResponsitory.Update(profileInDB);
                    _unitOfwork.Save();

                    if (!string.IsNullOrEmpty(profile.Avatar) && !string.IsNullOrEmpty(oldImage) && profile.Avatar != oldImage
                            && oldImage != "profile-icon.png")
                    {
                        _blobStorageService.DeleteBlobData(oldImage);
                    }

                    return true;
                }
                else
                {
                    EcommerceAPI.DataAccess.EFModel.Profile profileToupdate = new EcommerceAPI.DataAccess.EFModel.Profile()
                    {
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Address = profile.Address,
                        Avatar = "profile-icon.png", //default icon avatar
                        UserID = profile.UserID
                    };

                    if (profile.Avatar != null)
                    {
                        profileToupdate.Avatar = profile.Avatar;
                    }

                    return CreateProfile(profile);
                }
            }
            catch(Exception ex)
            {
                return false;
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
