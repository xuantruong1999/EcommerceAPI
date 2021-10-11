using EcommerceAPI.Model.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EcommerceAPI.Model
{
    public class UserViewModel 
    {
        public string Id { get; set; }
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
        [Compare("Password", ErrorMessage = "Confirmpassword and Password is not match")]
        public string ConfirmPassword { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class UserLoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public string ReturnURL { get; set; }
    }

    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public IList<string> Role { get; set; }
    }

    public class UserEditViewModel 
    {
        public UserEditViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
    }

    public class UserCreateViewModel
    {
        public UserCreateViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
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
        [Compare("Password", ErrorMessage = "Confirmpassword and Password is not match")]
        public string ConfirmPassword { get; set; }
        public DateTime CreateDate { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
    }
}
