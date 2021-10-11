using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.User
{
    public class UserRegisterModel
    {
        [Required, MinLength(6), MaxLength(14)]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
