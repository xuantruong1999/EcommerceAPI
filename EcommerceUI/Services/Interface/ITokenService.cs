using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.UI.Services.Interface
{
    public interface ITokenService
    {
        string GenerateTokenJWT(string userId, string secret, string issuer, string audience);
        bool IsValidToken(string token, string issuer, string key);
    }
   
}
