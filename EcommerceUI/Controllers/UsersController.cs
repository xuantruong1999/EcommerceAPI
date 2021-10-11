using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.Token;
using EcommerceAPI.Model.User;
using EcommerceAPI.UI.Controllers;
using EcommerceAPI.UI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
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
                        Avatar = baseUrl + avatar ?? "profile-icon.png",
                        Email = userDb.Email
                    };
                    string token = _tokenService.GenerateTokenJWT(userDb.Id);
                    string refreshToken = _tokenService.GenerateRefreshToken();
                    userDb.RefreshToken = refreshToken;
                    userDb.RefreshTokenTimeStamp = System.DateTime.Now.AddDays(7);
                    _unitOfWork.UserResponsitory.Update(userDb);
                    _unitOfWork.Save();
                    return Ok(new { user, token, refreshToken });

                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost, Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(JsonWebTokenModel jwt)
        {
            if(jwt is null)
            {
                return BadRequest();
            }

            //var userName = User.Identity.Name;
            var userID =  _tokenService.GetClaimsNamedId(jwt.AccessToken);
            
            var user = await _userManager.FindByIdAsync(userID);
            
            if(user == null || user.RefreshToken != jwt.RefreshToken || user.RefreshTokenTimeStamp <= System.DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenService.GenerateTokenJWT(user.Id);
            
            return new ObjectResult(new {
                token = newAccessToken,
            });
        }

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Cancelling jwt token
            await _tokenService.DeactivateCurrentAsync();
            if(!RevokeRefreshToken()) return Forbid();
            return NoContent();
        }

       
        private bool RevokeRefreshToken()
        {
            string token = _tokenService.GetCurrentAsync();
            var id = _tokenService.GetClaimsNamedId(token);
            var user = _unitOfWork.UserResponsitory.Find(u => u.Id == id).SingleOrDefault();
            if (user == null) return false;
            user.RefreshToken = null;
            _unitOfWork.UserResponsitory.Update(user);
            _unitOfWork.Save();
            return true;
        }
    }
}
