using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EcommerceAPI.Model
{
    public class UserViewModel
    {
           public string UserName { get; set; }
           public string Email { get; set; }
           public string PhoneNumber { get; set; }
    }
    public class UserRegisterViewModel
    {
        public UserRegisterViewModel()
        {
            CreateDate = DateTime.Now;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Confirmpassword and Password  is not match")]
        public string ConfirmPassword { get; set; }
        public DateTime CreateDate { get; set; }
        //SuperAdmin, Admin, Publisher, Normal User
    }

    public class UserLoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
        public string ReturnURL { get; set; }
    }


}
