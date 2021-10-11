using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.Model.Token
{
    public class JsonWebTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
