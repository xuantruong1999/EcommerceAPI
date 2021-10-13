using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class User : IdentityUser
    {
        [PersonalData]
        public Profile Profile { get; set; }
        [PersonalData]
        public string RefreshToken { get; set; }
        [PersonalData]
        public DateTime RefreshTokenTimeStamp { get; set; }
    }
}
