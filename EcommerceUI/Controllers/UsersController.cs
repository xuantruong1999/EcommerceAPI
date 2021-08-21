using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.Model.User;
using EcommerceAPI.UI.Controllers;
using EcommerceAPI.UI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public UsersController(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService) : base()
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async  Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                ResonseSuccessModel response = new ResonseSuccessModel()
                {
                    Status = 200,
                    Message = "Success",
                };

                string token = _tokenService.GenerateTokenJWT(user.Id, _config["JWT:Secret"], _config["JWT:ValidIssuer"], _config["JWT:ValidAudience"]);
                response.Token.Add("JWT", token);
                response.Data = user;
                return Ok(response);

            }
            else
            {
                var response = new ResponseModel()
                {
                    Status = 201,
                    Message = $"Can not find username: {loginModel.Username}"
                };

                return Ok(response);
            }
        }
    }
}
