using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceAPI.UI.Services.Interface
{
    public interface ITokenService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
        string GenerateTokenJWT(string userId);
        string GenerateRefreshToken();
        string GetClaimsNamedId(string token);
        string GetCurrentAsync();
    }
   
}
