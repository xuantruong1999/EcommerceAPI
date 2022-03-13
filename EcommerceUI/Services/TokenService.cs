using EcommerceAPI.UI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.UI.Services
{
    public class TokenService : ITokenService
    {
        //private readonly IDistributedCache _memoryCache;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public TokenService(IMemoryCache cache,
           IHttpContextAccessor httpContextAccessor,
           IConfiguration config)
        {
            _memoryCache = cache;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public bool IsCurrentActiveToken()
        =>  IsActiveAsync(GetCurrentAsync());

        public MemoryCacheEntryOptions DeactivateCurrentAsync()
            => DeactivateAsync(GetCurrentAsync());

        public bool IsActiveAsync(string token)
        {
            _memoryCache.TryGetValue(GetKey(token), out object key);
            return  key == null;
        }

        public MemoryCacheEntryOptions DeactivateAsync(string token)
        {
            var memoryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _memoryCache.Set(GetKey(token), memoryOption);
        }

        public string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.SingleOrDefault()?.Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";

        public string GenerateTokenJWT(string userId)
        {
            
            var encodeSecret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            //payload
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(12),
                Issuer = _config["JWT:ValidIssuer"],
                Audience = _config["JWT:ValidAudience"],
                SigningCredentials = new SigningCredentials(encodeSecret, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GetClaimsNamedId(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _config["JWT:ValidAudience"],
                ValidIssuer = _config["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]))
            };

            JwtSecurityToken principal = handler.ReadJwtToken(token);
            
            var namedId = principal.Claims.Where(c => c.Type == "nameid").Select(c => c.Value).SingleOrDefault();

            return namedId;
        }

        
    }
}
