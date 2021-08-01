using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class User : IdentityUser
    {
        public Profile Profile { get; set; }
    }
}
