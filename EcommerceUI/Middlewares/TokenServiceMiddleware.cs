using EcommerceAPI.UI.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EcommerceAPI.UI.Services
{
    public class TokenServiceMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;
        public TokenServiceMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenService.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
