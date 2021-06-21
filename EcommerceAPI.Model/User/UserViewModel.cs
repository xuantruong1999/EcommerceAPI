using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.User
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(60, MinimumLength = 6)]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        [Required]
        public string Phone { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        //SuperAdmin, Admin, Publisher, Normal User
        public string Permistion { get; set; }
    }
    public class UserLoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
