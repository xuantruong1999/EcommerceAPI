using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [Required]
        public User User { get; set; }

    }
}
