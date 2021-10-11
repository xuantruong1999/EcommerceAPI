using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Model.Profile
{
    public class ProfileViewModel
    {
        public string Avatar { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        public string UserID { get; set; }
    }
}
