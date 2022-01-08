using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcommerceAPI.Services
{
    public interface ICommonService
    {
        void DeleteImageExistes(string fileName);
        string UploadFile(IFormFile image);
    }
    public class CommonService : BaseService, ICommonService
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        public CommonService(IHostingEnvironment hostingEnvironment, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : base(unitOfWork, userManager, mapper) 
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Delete a image with name in static folder 
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteImageExistes(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            string pathFileToDelete = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "UserImages", fileName);

            if (System.IO.File.Exists(pathFileToDelete))
            {
                System.IO.File.Delete(pathFileToDelete);
            }
        }

        /// <summary>
        /// Upload a image to static folder
        /// </summary>
        /// <param name="image"></param>
        /// <returns>return a unique name or null</returns>
        public string UploadFile(IFormFile image)
        {
            if (image == null) return null;
            try
            {
                string fileName = Guid.NewGuid().ToString() + image.FileName;
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "ProductImages", fileName);
                var extension = new[] { "image/jpg", "image/png", "image/jpeg" };
                if (!extension.Contains(image.ContentType)) return null;
                using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(file);
                }
                return fileName;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
            catch (DriveNotFoundException ex)
            {
                throw ex;
            }
            catch (PathTooLongException ex)
            {
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                throw e;
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                throw e;
            }
            catch (IOException e)
            {
                throw e;
            }
        }
    }
}
