using Microsoft.Extensions.Caching.Memory;
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
        bool IsCurrentActiveToken();
        MemoryCacheEntryOptions DeactivateCurrentAsync();
        bool IsActiveAsync(string token);
        MemoryCacheEntryOptions DeactivateAsync(string token);
        string GenerateTokenJWT(string userId);
        string GenerateRefreshToken();
        string GetClaimsNamedId(string token);
        string GetCurrentAsync();
    }
   
}
