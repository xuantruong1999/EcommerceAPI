using AutoMapper;
using EcomerceAPI.Services;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EcommerceAPI.Services
{
    public interface ICommonSerivce
    {
        void DeleteImageExistes(string fileName);
    }
    public class CommonService : BaseService, ICommonSerivce
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        public CommonService(IHostingEnvironment hostingEnvironment, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : base(unitOfWork, userManager, mapper) 
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public void DeleteImageExistes(string fileName)
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
