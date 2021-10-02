using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.User;
using EcommerceAPI.UI.Controllers;
using EcommerceAPI.UI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWEB.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IConfiguration _config;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ITokenService _tokenService;
        public UsersController(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async  Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var userDb = await _userManager.FindByNameAsync(loginModel.Username);
            if (userDb != null && await _userManager.CheckPasswordAsync(userDb, loginModel.Password))
            {
                var avatar = _unitOfWork.ProfileResponsitory.Find(p => p.UserID == userDb.Id).FirstOrDefault()?.Avatar;
                var baseUrl = "http://127.0.0.1:5000/images/userimages/";
                UserApiModel user = new UserApiModel()
                {
                    Id = userDb.Id,
                    UserName = userDb.UserName,
                    PhoneNumber = userDb.PhoneNumber,
                    Avatar =  baseUrl + avatar ?? "profile-icon.png",
                    Email = userDb.Email
                };
                string token = _tokenService.GenerateTokenJWT(userDb.Id, _config["JWT:Secret"], _config["JWT:ValidIssuer"], _config["JWT:ValidAudience"]);
                return Ok(new { user , token });

            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _tokenService.DeactivateCurrentAsync();
            return NoContent();
        }
    }
}
