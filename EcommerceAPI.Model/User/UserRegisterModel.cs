using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.User
{
    public class UserRegisterModel
    {
        [Required, MinLength(6), MaxLength(14)]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        
        [Required(ErrorMessage ="Password is required")]
        [DataType("Password")]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        [DataType("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType("EmailAddress")]
        public string Email { get; set; }

    }
}
