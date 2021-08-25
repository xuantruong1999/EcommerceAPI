using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.Model.User
{
    public class UserApiModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
    }
}
